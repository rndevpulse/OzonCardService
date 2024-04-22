using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.Users;
using OzonCard.Identity.Application.Users.Commands;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

[Authorize(UserRole.Admin)]
public class AccountController : ApiController
{
    
    
    [HttpPost("[action]"), AllowAnonymous]
    public async Task<UserModel> Create(CreateUserModel model, CancellationToken ct = default)
    {
        var user = await Commands.Send(
            new CreateUserCommand(model.Email, model.Password, model.Roles),
            ct);
        return new UserModel(
            Guid.Parse(user.Id), model.Email, ArraySegment<UserOrganizationModel>.Empty);
    }
}