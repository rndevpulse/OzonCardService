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
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new BusinessException("Токен поврежден");
        if (!await _userManager.VerifyRefreshTokenAsync(user, HttpUtility.HtmlDecode(request.Refresh)))
            throw new BusinessException("Рефреш токен поврежден");
        await _userManager.RemoveRefreshTokenAsync(user);

        return await Authorization(user);
    }
}