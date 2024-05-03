using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Commands;

public record RemoveFileCommand(string Url) : ICommand<SaveFile>;