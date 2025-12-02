using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public record CustomerCategory(Guid CategoryId) : ValueObject;
