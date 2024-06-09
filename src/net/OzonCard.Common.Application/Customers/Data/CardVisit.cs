namespace OzonCard.Common.Application.Customers.Data;

public record CardVisit(
    string? Card,
    IEnumerable<CardVisitTransaction> Visits
);

public record CardVisitTransaction(DateTimeOffset Date, decimal Sum);
