using OzonCard.Common.Application.Organizations.Queries;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Handlers;

public class GetOrganizationsQueryHandler(
    IOrganizationRepository repository
) : IQueryHandler<GetOrganizationsQuery, IEnumerable<Organization>>
{

    public async Task<IEnumerable<Organization>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
    {
        return string.IsNullOrEmpty(request.User)
            ? await repository.GetItemsAsync(cancellationToken)
            : await repository.GetOrganizationsUser(request.User, cancellationToken);

    }
}