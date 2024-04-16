namespace OzonCard.Customer.Api.Models.Users;

public record UserModel(
    Guid Id,
    string Email, 
    IEnumerable<UserOrganizationModel> Organizations
);