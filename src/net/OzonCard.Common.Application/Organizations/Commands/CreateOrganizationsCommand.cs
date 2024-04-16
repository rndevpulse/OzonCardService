using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Commands;

public record CreateOrganizationsCommand(
    string Login,
    string Password,
    Guid UserId,
    string? User
) : ICommand<IEnumerable<Organization>>;
