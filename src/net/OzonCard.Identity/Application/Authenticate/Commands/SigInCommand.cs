using OzonCard.Common.Core;
using OzonCard.Identity.Application.Authenticate.Data;

namespace OzonCard.Identity.Application.Authenticate.Commands;

public record SigInCommand(
    string Email,
    string Password
) : ICommand<Auth>;
