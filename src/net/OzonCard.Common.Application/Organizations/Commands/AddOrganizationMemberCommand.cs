using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Commands;

public record AddOrganizationMemberCommand(
    Guid OrgId,
    Guid UserId,
    string User
):ICommand<Organization>;