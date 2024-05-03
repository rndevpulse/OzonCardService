using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files;

public interface IFileRepository : IRepository<SaveFile>
{
    Task<IEnumerable<SaveFile>> GetUserFiles(Guid userId, CancellationToken cancellationToken = default);
}