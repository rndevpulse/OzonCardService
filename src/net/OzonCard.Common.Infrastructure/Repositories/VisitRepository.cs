using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Infrastructure.Database;

namespace OzonCard.Common.Infrastructure.Repositories;

public class VisitRepository(
    InfrastructureContext context
) : IVisitRepository
{
    public async Task<IEnumerable<CustomerVisit>> GetVisitsAsync(Guid customer, CancellationToken ct = default)
    {
        return await context.Set<CustomerVisit>()
            .Where(x => x.Customer == customer)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<CustomerVisit>> GetVisitsAsync(Guid customer, DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default)
    {
        return await context.Set<CustomerVisit>()
            .Where(x => x.Customer == customer && x.Date >= from && x.Date <= to)
            .ToListAsync(ct);
    }

    public async Task AddAsync(params CustomerVisit[] visits)
    {
        await context.AddRangeAsync(visits);
    }

    public async Task<CustomerVisit?> GetVisitAsync(Guid customer, DateTimeOffset date, CancellationToken ct = default)
    {
        return await context.Set<CustomerVisit>()
            .FirstOrDefaultAsync(x => x.Customer == customer && x.Date == date, ct);
    }
}