using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Organizations.Queries;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Customer.Api.Models.Users;
using OzonCard.Identity.Application.Users.Commands;
using OzonCard.Identity.Application.Users.Queries;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

[Authorize(UserRole.Admin)]
public class AccountController : ApiController
{
    
    
    [HttpPost]
    public async Task<UserModel> Create(CreateUserModel model, CancellationToken ct = default)
    {
        var user = await Commands.Send(
            new CreateUserCommand(model.Email, model.Password, model.Roles),
            ct);
        return new UserModel(
            Guid.Parse(user.Id), model.Email, ArraySegment<UserOrganizationModel>.Empty);
    }
    
    
    [HttpGet]
    public async Task<IEnumerable<UserModel>> Members(CancellationToken ct = default)
    {
        var organizations = await Queries.Send(new GetOrganizationsQuery(null), ct);
        var users = await Queries.Send(new GetUsersQuery(), ct);
        var members = organizations
            .SelectMany(x => x.Members)
            .ToList();
        members.AddRange(users.Select(u=>new Member(new Guid(u.Id)){Name = u.Email ?? "UNKNOWN"}));
        var result = members
            .DistinctBy(x => x.UserId)
            .Select(x => new UserModel(
                x.UserId,
                x.Name,
                organizations
                    .Where(o => o.Members.Any(m => m.UserId == x.UserId))
                    .Select(o => new UserOrganizationModel(o.Id, o.Name))
                    .ToArray())
            );
        
        return result;
    }
}