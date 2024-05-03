using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OzonCard.Common.Core.Exceptions;


namespace OzonCard.Identity.Infrastructure.Jwt;


public class JwtGenerator : IJwtGenerator
{
    private class JwtSettings
    {
        public SigningCredentials Credentials { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int Duration { get; }

        public JwtSettings(string key, string issuer, string audience, int duration)
        {
            Issuer = issuer;
            Audience = audience;
            Duration = duration;
            Credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature);
        }
    }

    private readonly JwtSettings _settings;
    
    public JwtGenerator(IConfiguration configuration)
    {
        _settings = new(configuration["jwt:key"],
            configuration["jwt:issuer"],
            configuration["jwt:audience"],
            configuration.GetValue<int>("jwt:duration", 60));
    }

    public string CreateToken(string id, string? email, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, id),
            new (JwtRegisteredClaimNames.Sid, id),
            new (JwtRegisteredClaimNames.Email, email ?? "UNKNOWN"),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(x=>new Claim(ClaimsIdentity.DefaultRoleClaimType, x)));
        var std = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(_settings.Duration),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = _settings.Credentials,
        };
        var token = tokenHandler.CreateToken(std);
        return tokenHandler.WriteToken(token);
    }

    public string GetUserIdByToken(string access)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(access);
        if (jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid) is { } claim)
            return claim.Value;
        throw new BusinessException("Access token is corrupted");
    }

}