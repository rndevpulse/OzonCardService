using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Queries;

public record GetOrganizationsQuery(
    string? User
) : IQuery<IEnumerable<Organization>>;