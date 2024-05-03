using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Wallet(Guid Id, string Name, string ProgramType, string Type) : ValueObject;
