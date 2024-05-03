using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core.Exceptions;
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
        var refreshToken = await userManager.GenerateRefreshTokenAsync(user, "");
        var result = await userManager.SetRefreshTokenAsync(user, refreshToken);
        if (!result.Succeeded)
            throw new BusinessException("Error when saving refresh token");
        var userRoles = await userManager.GetRolesAsync(user);
        return new Auth(
            jwtGenerator.CreateToken(user.Id, user.Email, userRoles),
            refreshToken,
            userRoles
        );
    }
}