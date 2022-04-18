using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using System;
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
        public async Task<ActionResult<Authenticate_dto>> Token(Identity_vm auth)
        {
           
            var user = await _service.GetUser(auth.Email, auth.Password);
            var authenticate = await _service.Authenticate(user);
            if (authenticate == null)
                return Unauthorized();
            setTokenCookie(authenticate.RefreshToken);
            return new OkObjectResult(authenticate);
        }

        [HttpPost("refresh")]
        [Consumes("application/json")]
        public async Task<ActionResult<Authenticate_dto>> RefreshToken(RefrashToken_vm model)
        {

            var refreshToken = model?.Token ?? Request.Cookies["refreshToken"];
            var authenticate = await _service.RefreshToken(refreshToken);

            if (authenticate == null)
                return Unauthorized(new { message = "Invalid token" });
            setTokenCookie(authenticate.RefreshToken);
            return new OkObjectResult(authenticate);
        }

        [HttpPost("logout")]
        [Consumes("application/json")]
        public async Task<ActionResult> LogoutToken(RefrashToken_vm model)
        {
            var refreshToken = model?.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Ok(new { message = "Token is required" });

            var isLogout = await _service.LogoutToken(refreshToken);

            if (!isLogout)
                return Ok(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions()
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(Helpers.AuthOptions.LIFETIME_REFRESH)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
    
}
