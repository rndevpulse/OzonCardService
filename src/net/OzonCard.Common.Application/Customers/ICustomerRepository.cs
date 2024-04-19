using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetCustomersByCardsAsync(Guid organizationId, IEnumerable<string> tracks, CancellationToken ct = default);
    Task<IEnumerable<Customer>> SearchCustomersAsync(Guid organizationId, string name, string card, CancellationToken ct = default);
    Task<IEnumerable<Customer>> GetItemsAsync(Guid organizationId, CancellationToken ct = default);
}