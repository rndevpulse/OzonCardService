using OzonCard.Common.Application.Files.Commands;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Handlers;

public class SaveFileCommandHandler(
    IFileRepository repository
) : ICommandHandler<SaveFileCommand, SaveFile>
{
    public async Task<SaveFile> Handle(SaveFileCommand request, CancellationToken cancellationToken)
    {
        var named = request.FileName.Split(".");
        var file = new SaveFile(
            request.Id,
            named.Last().Trim().ToLower(),
            string.Join(".", named.Take(named.Length - 1)),
            request.UserId);
        await repository.AddAsync(file);
        return file;
    }
}