using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Application.Organizations.Queries;
using OzonCard.Customer.Api.Models.Organizations;
using OzonCard.Customer.Api.Models.Users;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;


[Authorize(UserRole.Basic)]
public class OrganizationController : ApiController
{

    [HttpPost, Authorize(UserRole.Admin)]
    public async Task<IEnumerable<OrganizationModel>> Create(string login, string password,
        CancellationToken ct = default)
    {
        var organizations = await Commands.Send(
            new CreateOrganizationsCommand(
                login,
                password,
                UserClaimSid,
                UserClaimEmail),
            ct);
        return Mapper.Map<IEnumerable<OrganizationModel>>(organizations);
    }


    [HttpGet]
    public async Task<IEnumerable<OrganizationModel>> Index(CancellationToken ct = default)
    {
        var organizations = await Queries.Send(
            new GetOrganizationsQuery(
                User.FindFirstValue(ClaimTypes.Email)),
            ct);
        return Mapper.Map<IEnumerable<OrganizationModel>>(organizations);
    }

    [HttpPut]
    public async Task<OrganizationModel> Update(Guid organizationId, CancellationToken ct = default)
    {
        var organization = await Commands.Send(
            new UpdateOrganizationCommand(organizationId),
            ct);
        return Mapper.Map<OrganizationModel>(organization);
    }
    
    
    
    [HttpGet("[action]"), Authorize(UserRole.Admin)]
    public async Task<IEnumerable<UserModel>> Members(CancellationToken ct = default)
    {
        var organizations = await Queries.Send(new GetOrganizationsQuery(null), ct);
    }
    
    [HttpPost("{organizationId:guid}"), Authorize(UserRole.Admin)]
    public async Task<UserModel> AddOrganization(Guid organizationId, Guid userId, CancellationToken ct = default)
    {
        
    }
    
    
    [HttpDelete("{organizationId:guid}"), Authorize(UserRole.Admin)]
    public async Task<UserModel> RemoveOrganization(Guid organizationId, Guid userId, CancellationToken ct = default)
    {
        
    }

}