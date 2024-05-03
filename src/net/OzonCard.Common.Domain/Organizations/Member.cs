using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Member(Guid UserId) : ValueObject
{
    public string Name { get; set; } = "";
  
};
