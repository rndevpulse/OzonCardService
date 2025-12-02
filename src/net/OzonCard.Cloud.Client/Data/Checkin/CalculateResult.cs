namespace OzonCard.Cloud.Client.Data.Checkin;

public record CalculateResult(
    IEnumerable<LoyaltyProgramResult> LoyaltyProgramResults
);

public record LoyaltyProgramResult(
    string Name,
    IEnumerable<LoyaltyProgramDiscount> Discounts,
    IEnumerable<LoyaltyProgramFreeProduct> FreeProducts,
    IEnumerable<LoyaltyProgramUpsaleProduct> Upsales
);

public record LoyaltyProgramDiscount(
    int Code,
    Guid? PositionId,
    decimal DiscountSum,
    decimal? Amount
);

public record LoyaltyProgramFreeProduct(
    IEnumerable<FreeProduct> Products
);

public record FreeProduct(
    Guid Id,
    string Code
);

public record LoyaltyProgramUpsaleProduct(
    IEnumerable<UpsaleProduct> Products
);

public record UpsaleProduct(
    Guid Id,
    string Code
);