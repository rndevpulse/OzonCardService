using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateCategoryCommand(
    Guid Id,
    Guid OrganizationId,
    Guid CategoryId,
    bool isRemove
) : ICommand<Customer>;
