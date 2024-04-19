namespace OzonCard.Common.Application.Customers.Data;

public record CustomerSearch(
    Guid Id,
    Guid BizId,
    string Name,
    string Card,
    string TabNumber,
    string Position,
    string Division,
    string Organization,
    decimal? Balance,
    decimal? Sum,
    decimal? Orders,
    IEnumerable<string> Categories,
    DateTime? LastVisit,
    int? DaysGrant
    );