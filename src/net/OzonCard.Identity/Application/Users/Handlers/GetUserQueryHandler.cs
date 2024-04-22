using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Identity.Application.Users.Queries;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Handlers;

public class GetUserQueryHandler(UserManager<User> userManager) : IQueryHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await userManager.FindByIdAsync(request.Id.ToString())
               ?? throw EntityNotFoundException.For<User>(request.Id);
    }
}