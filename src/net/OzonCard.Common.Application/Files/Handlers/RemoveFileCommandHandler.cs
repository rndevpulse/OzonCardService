using OzonCard.Common.Application.Files.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Handlers;

public class RemoveFileCommandHandler(
    IFileRepository repository
) : ICommandHandler<RemoveFileCommand>
{
    public async Task Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(
                request.Url.Split(".").First().Trim(), 
                out var id))
        {
            var file = await repository.GetItemAsync(id, cancellationToken);
            repository.Remove(file);
        }
        throw EntityNotFoundException.For<SaveFile>(request.Url);
    }
}