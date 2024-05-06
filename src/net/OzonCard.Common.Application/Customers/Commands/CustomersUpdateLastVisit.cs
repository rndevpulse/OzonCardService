using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public record CustomersUpdateLastVisit(
    Guid OrganizationId,
    IEnumerable<CustomerVisit> CustomersVisit
) : ICommand<IEnumerable<Customer>>;