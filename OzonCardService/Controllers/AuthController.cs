using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OzonCard.Common;
using OzonCardService.Helpers;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        IIdentityService _service;
        public AuthController(IIdentityService service) => _service = service;

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> Token(Identity_vm auth)
        {
            if (auth.Login == "" || auth.Password == "")
                return Unauthorized();
            var identity = await GetIdentity(auth.Login, auth.Password);

            if (identity == null)
                return Unauthorized();

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new OkObjectResult(new
            {
                access_token = encodedJwt
            });
        }

        private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
        {
            ClaimsIdentity identity = null;
            var user = await _service.GetUser(userName);
            if (user != null)
            {
                var passwordHash = UserHelper.GetHash(password);
                if (passwordHash == user.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    };
                    foreach (var rule in user.Rules.Split(','))
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, rule));

                    identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                }
            }
            return identity;
        }
        
    }
}
