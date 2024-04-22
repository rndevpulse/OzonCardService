using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Identity.Application.Authenticate.Commands;
using OzonCard.Identity.Application.Authenticate.Data;
using OzonCard.Identity.Domain;
using OzonCard.Identity.Infrastructure.Jwt;

namespace OzonCard.Identity.Application.Authenticate.Handlers;

public class SigInCommandHandler(
    UserManager<User> userManager, 
    IJwtGenerator jwtGenerator
) : AuthorizationBaseHandler(userManager, jwtGenerator), ICommandHandler<SigInCommand, Auth>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Auth> Handle(SigInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw EntityNotFoundException.For<User>(request.Email);
        
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new BusinessException("Non correct password authorization");
        
        return await Authorization(user);
    }
}