using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Handlers;

public class RemoveOrganizationMemberCommandHandler(
    IOrganizationRepository repository
) : ICommandHandler<RemoveOrganizationMemberCommand, Organization>
{
    public async Task<Organization> Handle(RemoveOrganizationMemberCommand request, CancellationToken cancellationToken)
    {
        var organization = await repository.GetItemAsync(request.OrgId, cancellationToken);
        organization.RemoveMember(request.UserId);
        return organization;
    }
}