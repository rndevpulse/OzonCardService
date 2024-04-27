using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerUpdateCommand(
    Guid Id,
    string Name,
    string TabNumber,
    string Position,
    string Division
) : ICommand<Customer>;