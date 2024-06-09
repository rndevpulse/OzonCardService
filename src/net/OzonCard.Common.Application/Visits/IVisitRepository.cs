using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Visits;

public interface IVisitRepository
{
    Task<IEnumerable<CustomerVisit>> GetVisitsAsync(Guid customer, CancellationToken ct = default);
    Task<IEnumerable<CustomerVisit>> GetVisitsAsync(Guid customer, DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default);
    Task AddAsync(params CustomerVisit[] visits);
    Task<CustomerVisit?> GetVisitAsync(Guid customer, DateTimeOffset date, CancellationToken ct = default);

}