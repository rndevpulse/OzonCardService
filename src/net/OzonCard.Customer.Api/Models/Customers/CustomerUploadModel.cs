namespace OzonCard.Customer.Api.Models.Customers;

public record CustomerUploadModel(
    Guid OrganizationId,
    IEnumerable<Guid> CategoriesId,
    Guid ProgramId,
    decimal Balance,
    string Name,
    string Card
);