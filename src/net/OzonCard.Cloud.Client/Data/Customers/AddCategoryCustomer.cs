namespace OzonCard.Cloud.Client.Data.Customers;

public record AddCategoryCustomer(
    Guid CategoryId,
    Guid CustomerId,
    Guid OrganizationId
);