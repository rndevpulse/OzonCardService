using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Files.Commands;

public record RemoveFileCommand(string Url):ICommand;