using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core;
using OzonCard.Identity.Application.Authenticate.Commands;
using OzonCard.Identity.Domain;
using OzonCard.Identity.Infrastructure.Security;

namespace OzonCard.Identity.Application.Authenticate.Handlers;

public class LogoutCommandHandler(
    UserManager<User> userManager
) : ICommandHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return;
        await userManager.RemoveRefreshTokenAsync(user, request.Refresh);
    }
}