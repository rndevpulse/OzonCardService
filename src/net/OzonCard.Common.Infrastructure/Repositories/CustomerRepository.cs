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
    public async Task<IEnumerable<Customer>> GetCustomersByCardsAsync(Guid organizationId, IEnumerable<string> tracks, CancellationToken ct = default)
    {
        var customers = await GetQuery()
            .Where(x => x.OrgId == organizationId)
            .ToListAsync(ct);
        return customers
            .Where(x => x.Cards.Any(c => tracks.Contains(c.Track)))
            .ToList();
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(Guid organizationId, string name, string card, CancellationToken ct = default)
    {
        var queqry = GetQuery();
        queqry = string.IsNullOrEmpty(name)
            ? queqry.Where(x => x.Cards.Any(c => c.Number == card))
            : queqry.Where(x => EF.Functions.Like(x.Name, $"%{name}%"));
        return await queqry.ToListAsync(ct);
    }

    public async Task<IEnumerable<Customer>> GetItemsAsync(Guid organizationId, CancellationToken ct = default)
    {
        return await GetQuery()
            .Where(x => x.OrgId == organizationId)
            .ToListAsync(ct);
    }
}