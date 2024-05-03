namespace OzonCard.Biz.Client.Models.Organizations;

public class ProgramDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime? ServiceFrom { get; set; }
    public DateTime? ServiceTo { get; set; }
    public IEnumerable<WalletDto> Wallets { get; set; } = ArraySegment<WalletDto>.Empty;
}