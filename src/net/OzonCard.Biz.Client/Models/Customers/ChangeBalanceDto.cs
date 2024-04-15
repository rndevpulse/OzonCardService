namespace OzonCard.Biz.Client.Models.Customers;

public record ChangeBalanceDto(
    Guid CustomerId,
    Guid OrganizationId,
    Guid WalletId,
    decimal Sum
);