using OzonCard.Common.Application.Files.Queries;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Handlers;

public class GetFilesQueryHandler(
    IFileRepository repository
) : IQueryHandler<GetFilesQuery, IEnumerable<SaveFile>>
{
    public Task<IEnumerable<SaveFile>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        return repository.GetUserFiles(request.UserId, cancellationToken);
    }
}