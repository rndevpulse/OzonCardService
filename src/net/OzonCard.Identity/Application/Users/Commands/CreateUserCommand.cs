using OzonCard.Common.Core;
using OzonCard.Identity.Domain;

namespace OzonCard.Identity.Application.Users.Commands;

public record CreateUserCommand(
    string Email,
    string Password,
    IEnumerable<string> Rules
):ICommand<User>;