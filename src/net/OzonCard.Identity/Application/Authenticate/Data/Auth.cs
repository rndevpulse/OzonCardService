namespace OzonCard.Identity.Application.Authenticate.Data;

public record Auth(
    string Access,
    string Refresh,
    IEnumerable<string> Rules
);