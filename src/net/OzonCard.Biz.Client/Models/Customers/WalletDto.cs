namespace OzonCard.Biz.Client.Models.Customers;

public record WalletDto(
    decimal Balance,
    WalletInfoDto Wallet
);
