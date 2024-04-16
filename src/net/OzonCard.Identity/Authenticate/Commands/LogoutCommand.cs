using OzonCard.Common.Core;

namespace OzonCard.Identity.Authenticate.Commands;

public record LogoutCommand(string? token):ICommand;