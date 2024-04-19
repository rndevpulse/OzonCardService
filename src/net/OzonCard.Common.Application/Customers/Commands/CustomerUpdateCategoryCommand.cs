using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateCategoryCommand(
    Guid Id,
    Guid OrganizationId,
    Guid CategoryId,
    bool isRemove
) : ICommand<string>;
