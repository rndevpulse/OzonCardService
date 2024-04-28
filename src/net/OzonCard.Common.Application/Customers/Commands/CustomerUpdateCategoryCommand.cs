using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateCategoryCommand(
    Guid Id,
    Guid OrganizationId,
    IEnumerable<Guid> Categories,
    bool IsRemove
) : ICommand<IEnumerable<string>>;
