using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateBalanceCommand(
    Guid Id,
    Guid OrganizationId,
    Guid ProgramId,
    bool isIncrement,
    decimal Balance
) : ICommand<Customer>;