using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Application.Properties;
using OzonCard.Common.Domain.Props;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Repositories.Abstractions;

namespace OzonCard.Common.Infrastructure.Repositories;

public class PropertiesRepository(
    InfrastructureContext context
) : RepositoryBase<Property>(context), IPropertiesRepository
{
    public async Task<IEnumerable<Property>> GetItemsAsync(Guid reference, PropType type, CancellationToken ct = default)
    {
        return await GetQuery()
            .Where(x => x.Reference == reference && x.Type == type)
            .ToListAsync(ct);
    }

}