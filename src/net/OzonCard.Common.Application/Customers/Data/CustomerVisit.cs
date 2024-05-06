namespace OzonCard.Common.Application.Customers.Data;

public record CustomerVisit(
    string? Card,
    DateTimeOffset LastVisitDate,
    int DaysGrant
);