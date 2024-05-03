using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateBalanceCommand(
    Guid Id,
    Guid ProgramId,
    decimal Balance
) : ICommand<decimal>;