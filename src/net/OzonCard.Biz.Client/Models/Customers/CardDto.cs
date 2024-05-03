namespace OzonCard.Biz.Client.Models.Customers;

public record CardDto(
    Guid Id,
    bool IsActivated,
    string Number,
    string Track
);