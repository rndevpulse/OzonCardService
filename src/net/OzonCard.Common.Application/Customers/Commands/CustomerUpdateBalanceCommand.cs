using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateBalanceCommand(
    Guid Id,
    Guid OrganizationId,
    Guid ProgramId,
    bool isIncrement,
    decimal Balance
) : ICommand<decimal>;