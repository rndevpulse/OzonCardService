using OzonCard.Common.Core;
using OzonCard.Identity.Authenticate.Data;

namespace OzonCard.Identity.Authenticate.Commands;

public record SigInCommand(
    string Email,
    string Password
) : ICommand<Auth>;
