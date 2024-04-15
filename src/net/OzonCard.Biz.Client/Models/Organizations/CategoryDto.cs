namespace OzonCard.Biz.Client.Models.Organizations;

public record CategoryDto(
    Guid Id,
    bool IsActive,
    string Name
);