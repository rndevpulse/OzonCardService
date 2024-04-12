using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public record CustomerWallet(
    Guid Id,
    double Balance,
    string Name,
    string ProgramType,
    string Type
) : ValueObject;
