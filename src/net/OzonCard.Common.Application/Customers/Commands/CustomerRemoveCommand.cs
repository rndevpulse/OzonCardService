using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomerRemoveCommand(Guid Id) : ICommand<Customer>;