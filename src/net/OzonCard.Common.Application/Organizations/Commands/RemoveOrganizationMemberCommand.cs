using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Commands;

public record RemoveOrganizationMemberCommand(
    Guid OrgId,
    Guid UserId
):ICommand<Organization>;