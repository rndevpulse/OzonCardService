using OzonCard.Common.Application.Visits;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers;

public class CoreCustomerContext(
    Customer customer,
    IVisitRepository repository
) : ICustomerContext
{
    public Task<IEnumerable<CustomerVisit>> GetVisitsAsync(CancellationToken ct = default) =>
        repository.GetVisitsAsync(customer.Id, ct);

    public Task<IEnumerable<CustomerVisit>> GetVisitsAsync(DateTimeOffset from, DateTimeOffset to,
        CancellationToken ct = default) =>
        repository.GetVisitsAsync(customer.Id, from, to, ct);

    public async Task<IEnumerable<CustomerVisit>> UpdateAsync(IEnumerable<CustomerVisit> times, CancellationToken ct = default)
    {
        var result = new List<CustomerVisit>();
        foreach (var time in times)
        {
            var visit = await repository.GetVisitAsync(customer.Id, time.Date, ct);
            if (visit != null)
                continue;
            visit = time;
            result.Add(visit);
        }

        await repository.AddAsync(result.ToArray());
        return result;
    }
}