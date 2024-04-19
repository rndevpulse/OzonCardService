using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Repositories.Abstractions;

namespace OzonCard.Common.Infrastructure.Repositories;

public class CustomerRepository(
    InfrastructureContext context
) : RepositoryBase<Customer>(context), ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetCustomersByCards(Guid organizationId, IEnumerable<string> tracks, CancellationToken ct = default)
    {
        return await GetQuery()
            .Where(x => x.OrgId == organizationId && x.Cards.Any(c => tracks.Contains(c.Track)))
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Customer>> SearchCustomers(Guid organizationId, string name, string card, CancellationToken ct = default)
    {
        return await GetQuery()
            .Where(x => x.OrgId == organizationId)
            .Where(x => x.Name == name || x.Cards.Any(c => c.Number == card))
            .ToListAsync(ct);
    }
}