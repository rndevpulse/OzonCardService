namespace OzonCard.Identity.Application.Authenticate.Data;

public record JwtToken(
    string Token,
    DateTime? Expired
);