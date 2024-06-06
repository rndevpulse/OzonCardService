using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public record CustomerVisit : ValueObject
{
    public Guid Customer { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Sum { get; set; }
}