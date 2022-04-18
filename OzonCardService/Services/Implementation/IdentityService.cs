using Microsoft.IdentityModel.Tokens;
using OzonCard.Common;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using OzonCardService.Helpers;
using OzonCardService.Models.DTO;
using OzonCardService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OzonCardService.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
		IIdentityRepository _repository;

		public IdentityService(IIdentityRepository repository) => _repository = repository;

		public async Task<User> GetUser(string mail, string password)
		{
			return await _repository.GetUser(mail, UserHelper.GetHash(password));
		}

        ClaimsIdentity GetIdentity(User user)
        {
            ClaimsIdentity identity = null;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                };
                foreach (var rule in user.Rules.Split(','))
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, rule));

                identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }
            return identity;
        }

        string NetJwtToken(ClaimsIdentity identity)
        {
            
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: DateTime.UtcNow,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)));
        }
        string NetJwtToken(User user)
        {
            return NetJwtToken(GetIdentity(user));
        }
        RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = UserHelper.GetCsp(),
                Expires = DateTime.UtcNow.AddDays(AuthOptions.LIFETIME_REFRESH),
                Created = DateTime.UtcNow
            };
        }


        public async Task<Authenticate_dto> Authenticate(User user)
        {
            var identity = GetIdentity(user);
            if (identity == null)
                return null;
            var refreshToken = GenerateRefreshToken();
            await _repository.AddRefreshToken(user, refreshToken);
            return new Authenticate_dto
            {
                Email = user.Mail,
                Token = NetJwtToken(identity),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<Authenticate_dto> RefreshToken(string refreshToken)
        {
            var user = await _repository.GetUser(refreshToken);
            if (user == null)
                return null;
            var tokenUserRefresh = user.RefreshTokens
                .Single(x => x.Token == refreshToken);
            if (!tokenUserRefresh.isActive) 
                return null;
            //генерируем новый токен рефреш и указываем его в старом
            var newRefreshToken = GenerateRefreshToken();
            tokenUserRefresh.Revoked = DateTime.UtcNow;
            tokenUserRefresh.ReplacedByToken = newRefreshToken.Token;
            await _repository.AddRefreshToken(user, newRefreshToken);

            return new Authenticate_dto
            {
                RefreshToken = newRefreshToken.Token,
                Token = NetJwtToken(user),
                Email = user.Mail
            };

        }

        public async Task<bool> LogoutToken(string refreshToken)
        {
            var user = await _repository.GetUser(refreshToken);
            if (user == null)
                return false;
            var tokenUserRefresh = user.RefreshTokens
                .Single(x => x.Token == refreshToken);
            if (!tokenUserRefresh.isActive)
                return false;
            tokenUserRefresh.Revoked = DateTime.UtcNow;
            await _repository.AddRefreshToken(user);
            return true;
        }
    }
}
