namespace OzonCard.Customer.Api.Models.Auth;

public record TokenModel(
    string Access,
    string? Refresh = null
);