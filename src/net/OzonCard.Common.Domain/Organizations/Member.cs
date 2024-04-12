using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Member : ValueObject
{
    public string Name { get; init; }
    public string Rules { get; set; }
};
