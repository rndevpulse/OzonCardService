using Microsoft.AspNetCore.Identity;
using OzonCard.Common.Core;
using OzonCard.Identity.Application.Users.Queries;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Handlers;

public class GetUsersQueryHandler(
    UserManager<User> userManager
) : IQueryHandler<GetUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userManager.GetUsersInRoleAsync(UserRole.Basic);
    }
}