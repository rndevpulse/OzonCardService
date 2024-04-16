namespace OzonCard.Identity.Authenticate.Data;

public record Auth(
    string Access,
    string Refresh,
    IEnumerable<string> Rules
);