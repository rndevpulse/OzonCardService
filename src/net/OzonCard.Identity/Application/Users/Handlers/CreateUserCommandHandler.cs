using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Identity.Application.Users.Commands;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Handlers;

public class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger,
    UserManager<User> userManager
) : ICommandHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is { EmailConfirmed: false })
            await userManager.DeleteAsync(user);
        
        //try save new user
        user = new User { UserName = request.Email, Email = request.Email, EmailConfirmed = true};
        var result = await userManager.CreateAsync(
            user,
            request.Password
        );
        if (!result.Succeeded)
            throw new BusinessException(string.Join(". ", result.Errors.Select(x => x.Description)));
        
        //try add roles
        
        result = await userManager.AddToRolesAsync(user, TryParseRoles(request.Roles));
        
        if (!result.Succeeded)
            throw new BusinessException(string.Join(". ", result.Errors.Select(x => x.Description)));

        logger.LogInformation($"Created new user {request.Email}");
        return user;

    }

    private readonly string[] _roles = [UserRole.Basic, UserRole.Report, UserRole.Admin];
    private IEnumerable<string> TryParseRoles(IEnumerable<string> roles)
    {
        var result = new List<string> { UserRole.Basic };
        result.AddRange(roles.Select(x => _roles.Contains(x) ? x : UserRole.Basic));
        return result.Distinct().ToArray();
    }
}