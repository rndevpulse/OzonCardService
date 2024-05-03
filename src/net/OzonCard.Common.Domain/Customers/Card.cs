using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public record Card(
    string Track, 
    string Number, 
    DateTime Created
) : ValueObject;
