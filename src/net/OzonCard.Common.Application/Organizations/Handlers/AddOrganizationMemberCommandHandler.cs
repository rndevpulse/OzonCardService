using OzonCard.Common.Application.Organizations.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Handlers;

public class AddOrganizationMemberCommandHandler(
    IOrganizationRepository repository
) : ICommandHandler<AddOrganizationMemberCommand, Organization>
{
    public async Task<Organization> Handle(AddOrganizationMemberCommand request, CancellationToken cancellationToken)
    {
        var organization = await repository.GetItemAsync(request.OrgId, cancellationToken);
        organization.AddOrUpdateMember(request.UserId, request.User);
        return organization;
    }
}