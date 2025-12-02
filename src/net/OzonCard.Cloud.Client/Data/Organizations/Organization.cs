namespace OzonCard.Cloud.Client.Data.Organizations;

public record Organization(
    Guid Id,
    string Name,
    string? Code
);