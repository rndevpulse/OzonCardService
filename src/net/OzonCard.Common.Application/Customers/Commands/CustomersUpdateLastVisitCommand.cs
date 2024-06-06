using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomersUpdateLastVisitCommand(
    Guid OrganizationId,
    IEnumerable<CardVisit> CustomersVisit
) : ICommand<IEnumerable<Customer>>;

