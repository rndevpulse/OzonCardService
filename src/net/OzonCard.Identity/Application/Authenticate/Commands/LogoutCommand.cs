using OzonCard.Common.Core;

namespace OzonCard.Identity.Application.Authenticate.Commands;

public record LogoutCommand(string Access, string Refresh):ICommand;