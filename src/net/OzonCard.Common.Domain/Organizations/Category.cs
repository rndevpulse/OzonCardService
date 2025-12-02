using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Category : ValueObject
{
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
}