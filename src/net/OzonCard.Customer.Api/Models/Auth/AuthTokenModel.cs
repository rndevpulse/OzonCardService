namespace OzonCard.Customer.Api.Models.Auth;

public record AuthTokenModel(
    string Access,
    IEnumerable<string> Rules
);
