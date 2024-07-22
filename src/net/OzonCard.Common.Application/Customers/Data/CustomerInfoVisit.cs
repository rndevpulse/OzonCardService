namespace OzonCard.Common.Application.Customers.Data;

public record CustomerInfoVisit(
    Guid Id, 
    string Name, 
    DateTime CreatedAt
);