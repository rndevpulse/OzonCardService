using OzonCard.Common.Core;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Organizations.Commands;

public record CreateOrganizationsCommand(
    string Endpoint,
    string Login,
    string Password,
    string Token,
    Guid UserId,
    string? User
) : ICommand<IEnumerable<Organization>>;