using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Organizations.Queries;
using OzonCard.Customer.Api.Models.Organizations;

namespace OzonCard.Customer.Api.Controllers.Bot;

public class OrganizationController : BotController
{
    [HttpGet]
    public async Task<IEnumerable<OrganizationModel>> Index(CancellationToken ct = default)
    {
        var organizations = await Queries.Send(
            new GetOrganizationsQuery(null),
            ct);
        return Mapper.Map<IEnumerable<OrganizationModel>>(organizations);
    }
}