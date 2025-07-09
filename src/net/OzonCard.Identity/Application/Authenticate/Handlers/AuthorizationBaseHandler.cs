using Microsoft.AspNetCore.Identity;
using OzonCard.Identity.Application.Authenticate.Data;
using OzonCard.Identity.Domain;
using OzonCard.Identity.Infrastructure.Jwt;
using OzonCard.Identity.Infrastructure.Security;

namespace OzonCard.Identity.Application.Authenticate.Handlers;

public abstract class AuthorizationBaseHandler(
    UserManager<User> userManager,
    IJwtGenerator jwtGenerator
)
{
    protected async Task<Auth> Authorization(User user)
    {
        var refreshToken = await userManager.GenerateRefreshTokenAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);
        var jwt = jwtGenerator.CreateToken(user.Id, user.Email, userRoles);
        return new Auth(
            jwt,
            refreshToken,
            userRoles,
            null
        );
    }
}