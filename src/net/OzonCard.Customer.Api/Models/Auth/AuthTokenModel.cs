namespace OzonCard.Customer.Api.Models.Auth;

public record AuthTokenModel(
    string Access,
    string Refresh,
    IEnumerable<string> Rules
);
