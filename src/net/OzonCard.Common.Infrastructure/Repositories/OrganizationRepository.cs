using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Repositories.Abstractions;

namespace OzonCard.Common.Infrastructure.Repositories;

public class OrganizationRepository(InfrastructureContext context) 
    : RepositoryBase<Organization>(context), IOrganizationRepository
{
    
    
    public async Task<IEnumerable<Organization>> GetOrganizationsUser(string user, CancellationToken ct = default)
    {
        return await GetQuery()
            .Where(x => x.Members.Any(m => m.Name == user))
            .ToListAsync(ct);
    }
}