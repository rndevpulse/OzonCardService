using System.Web;
using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Identity.Application.Authenticate.Commands;
using OzonCard.Identity.Application.Authenticate.Data;
using OzonCard.Identity.Domain;
using OzonCard.Identity.Infrastructure.Jwt;
using OzonCard.Identity.Infrastructure.Security;

namespace OzonCard.Identity.Application.Authenticate.Handlers;

public class UpdateRefreshTokenCommandHandler(
    UserManager<User> userManager,
    IJwtGenerator jwtGenerator
) : AuthorizationBaseHandler(userManager, jwtGenerator), ICommandHandler<UpdateRefreshTokenCommand, Auth>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Auth> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null || !await _userManager.VerifyRefreshTokenAsync(user, request.Refresh))
            throw new BusinessException("Необходимо пройти авторизацию для продолжения работы");
        await _userManager.RemoveRefreshTokenAsync(user, request.Refresh);

        return await Authorization(user);
    }
}