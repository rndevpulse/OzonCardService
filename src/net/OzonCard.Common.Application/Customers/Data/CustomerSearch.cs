using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Application.Customers.Data;

public record CustomerSearch(
    Guid Id,
    Guid BizId,
    Guid ProgramId,
    string Name,
    string Card,
    string TabNumber,
    string Position,
    string Division,
    string Organization,
    decimal? Balance,
    decimal? Sum,
    decimal? Orders,
    IEnumerable<Category> Categories,
    int? DaysGrant,
    DateTimeOffset? LastVisit,
    DateTimeOffset? Updated,
    DateTimeOffset? CreatedAt
);