namespace OzonCard.Biz.Client.Models.Customers;

public record WalletInfoDto(
    Guid Id,
    string Name,
    string Type,
    string ProgramType
);