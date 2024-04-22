namespace OzonCard.Identity.Infrastructure.Jwt;

public interface IJwtGenerator
{
    string CreateToken(string id, string? email , IEnumerable<string> roles);
    string GetUserIdByToken(string access);
}