using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Repositories.Abstractions;

namespace OzonCard.Common.Infrastructure.Repositories;

public class FileRepository(
    InfrastructureContext context
) : RepositoryBase<SaveFile>(context), IFileRepository
{
    public async Task<IEnumerable<SaveFile>> GetUserFiles(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetQuery()
            .Where(x => x.User == userId)
            .ToListAsync(cancellationToken);
    }
}