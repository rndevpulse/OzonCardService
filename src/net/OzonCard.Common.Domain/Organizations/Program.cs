using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Organizations;

public record Program(Guid Id) : ValueObject
{
    private readonly ICollection<Wallet> _wallets = new List<Wallet>();
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
    public IEnumerable<Wallet> Wallets => _wallets;

    public void AddOrUpdateWallet(Wallet wallet)
    {
        var item = _wallets.FirstOrDefault(x => x.Id == wallet.Id);
        if (item == null)
            _wallets.Add(wallet);
    }
}