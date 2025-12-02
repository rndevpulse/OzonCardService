namespace OzonCard.Cloud.Client.Data.Customers;

public record AddCardCustomer(
    Guid CustomerId,
    Guid OrganizationId,
    string CardNumber,
    string CardTrack
);