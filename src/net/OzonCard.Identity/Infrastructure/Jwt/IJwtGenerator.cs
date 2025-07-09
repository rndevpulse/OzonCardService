
namespace OzonCard.Identity.Infrastructure.Jwt;

public interface IJwtGenerator
{
    string CreateToken(string id, string? email , IEnumerable<string> roles);
    string GetClaimValue(string token, string claim);
}