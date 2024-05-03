
namespace OzonCard.Customer.Api.Models.Users;

public record CreateUserModel(
    string Email,
    string Password,
    IEnumerable<string> Roles
);