namespace OzonCard.Biz.Client.Models.Customers;

public record CustomerDto(
    Guid? Id,
    string Name,
    string? MagnetCardTrack,
    string? MagnetCardNumber
);