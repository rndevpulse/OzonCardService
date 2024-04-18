﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Application.Organizations.Queries;
using OzonCard.Customer.Api.Models.Organizations;
using OzonCard.Customer.Api.Models.Users;
using OzonCard.Identity.Application.Users.Queries;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;


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
        var result = organizations
            .SelectMany(x => x.Members)
            .DistinctBy(x => x.UserId)
            .ToArray()
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
    
    [HttpPost("{organizationId:guid}/members"), Authorize(UserRole.Admin)]
    public async Task<UserModel> AddOrganization(Guid organizationId, Guid userId, CancellationToken ct = default)
    {
        var user = await Queries.Send(new GetUserQuery(userId), ct);
        var organization = await Commands.Send(
            new AddOrganizationMemberCommand(organizationId, userId, user.Email ?? "Unknown"),
            ct);
        return new UserModel(
            userId,
            user.Email ?? "Unknown",
            new[] { new UserOrganizationModel(organization.Id, organization.Name) }
        );
    }
    
    
    [HttpDelete("{organizationId:guid}/members"), Authorize(UserRole.Admin)]
    public async Task<UserModel> RemoveOrganization(Guid organizationId, Guid userId, CancellationToken ct = default)
    {
        var user = await Queries.Send(new GetUserQuery(userId), ct);
        var organization = await Commands.Send(
            new RemoveOrganizationMemberCommand(organizationId,  userId),
            ct);
        return new UserModel(
            userId,
            user.Email ?? "Unknown",
            new[] { new UserOrganizationModel(organization.Id, organization.Name) }
        );
    }

}