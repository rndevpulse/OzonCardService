using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public class Customer : AggregateRoot
{
    private readonly ICollection<CustomerWallet> _wallets = new List<CustomerWallet>();
    private readonly ICollection<Card> _cards = new List<Card>();
    public string Name { get; set; }
    public string? Phone { get; private set; }
    public string? TabNumber { get; set; }
    public string? Position { get; set; }
    public string? Division { get; set; }
    public bool IsActive { get; private set; }
    public string? Comment { get; private set; }
    public Guid BizId { get; private set; }
    public Guid OrgId { get; private set; }

    public IEnumerable<Card> Cards => _cards;
    public IEnumerable<CustomerWallet> Wallets => _wallets;

    public Customer(
        Guid id,
        string name, 
        Guid bizId,
        Guid orgId,
        bool isActive = true,
        string? phone = null, 
        string? tabNumber  = null, 
        string? position = null,
        string? division = null, 
        string? comment = null) : base(id)
    {
        Name = name;
        Phone = phone;
        TabNumber = tabNumber;
        Position = position;
        Division = division;
        IsActive = isActive;
        Comment = comment;
        BizId = bizId;
        OrgId = orgId;
    }

    public void TryAddCard(string track, string number)
    {
        var card = _cards.FirstOrDefault(x => x.Track == track);
        if (card == null)
            _cards.Add(new Card(track, number, DateTime.UtcNow));
    }

    public void TryAddWallet(
        Guid walletId,
        string name,
        string programType,
        string type)
    {
        var wallet = _wallets.FirstOrDefault(x => x.WalletId == walletId);
        if (wallet == null)
            _wallets.Add(new CustomerWallet(walletId, 0, name, programType, type));
    }
}