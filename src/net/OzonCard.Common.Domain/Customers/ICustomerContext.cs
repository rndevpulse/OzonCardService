namespace OzonCard.Common.Domain.Customers;

public interface ICustomerContext
{
    Task<IEnumerable<CustomerVisit>> GetVisitsAsync(CancellationToken ct = default);
    Task<IEnumerable<CustomerVisit>> GetVisitsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default);
    Task<IEnumerable<CustomerVisit>> UpdateAsync(IEnumerable<CustomerVisit> times, CancellationToken ct = default);

}