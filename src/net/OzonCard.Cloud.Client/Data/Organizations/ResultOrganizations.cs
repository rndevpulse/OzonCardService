namespace OzonCard.Cloud.Client.Data.Organizations;

public record ResultOrganizations(
    Guid CorrelationId,
    IEnumerable<Organization> Organizations
);