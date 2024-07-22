using OzonCard.Biz.Client.Models.Organizations;

namespace OzonCard.Biz.Client.Models.Customers;

public class CustomerInfoDto
{
    public IEnumerable<CardDto> Cards { get; set; } = ArraySegment<CardDto>.Empty;
    public IEnumerable<CategoryDto> Categories { get; set; } = ArraySegment<CategoryDto>.Empty;
    public IEnumerable<WalletDto> WalletBalances { get; set; }

    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? Phone { get; set; }
}