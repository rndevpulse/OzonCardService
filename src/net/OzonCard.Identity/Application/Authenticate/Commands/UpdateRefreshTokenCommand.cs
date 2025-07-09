using OzonCard.Common.Core;
using OzonCard.Identity.Application.Authenticate.Data;

namespace OzonCard.Identity.Application.Authenticate.Commands;

public record UpdateRefreshTokenCommand(string Token, string Refresh):ICommand<Auth>;