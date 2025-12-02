using OzonCard.Common.Domain.Abstractions;
using OzonCard.Common.Domain.Organizations;

namespace OzonCard.Common.Domain.Customers;

public class Customer : AggregateRoot
{
    // private readonly ICollection<CustomerWallet> _wallets = new List<CustomerWallet>();
    private readonly ICollection<Card> _cards = new List<Card>();
    private readonly ICollection<CustomerCategory> _categories = new List<CustomerCategory>();
    public string Name { get; set; }
    public string? Phone { get; private set; }
    public string? TabNumber { get; set; }
    public string? Position { get; set; }
    public string? Division { get; set; }
    
    public DateTimeOffset? CreatedBiz { get; set; }
    public bool IsActive { get; private set; }
    public string? Comment { get; private set; }
    public Guid BizId { get; private set; }
    public Guid OrgId { get; private set; }
    public DateTimeOffset LastVisit { get; set; }
    public ICustomerContext Context { get; set; } = null!;

    public IEnumerable<Card> Cards => _cards;
    public IEnumerable<CustomerCategory> Categories => _categories;
    // public IEnumerable<CustomerWallet> Wallets => _wallets;

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
            _cards.Add(new Card(track, number, DateTimeOffset.UtcNow));
    }

    public void AddCategory(Category category)
    {
        if (_categories.Any(x => x.CategoryId == category.CategoryId))
            return;
        _categories.Add(new CustomerCategory(category.CategoryId));
    }

    public void RemoveCategory(Category category)
    {
        if (_categories.FirstOrDefault(x=>x.CategoryId == category.CategoryId) is {} value)
            _categories.Remove(value);
    }

    // public void TryAddWallet(
    //     Guid walletId,
    //     string name,
    //     string programType,
    //     string type)
    // {
    //     var wallet = _wallets.FirstOrDefault(x => x.WalletId == walletId);
    //     if (wallet == null)
    //         _wallets.Add(new CustomerWallet(walletId, 0, name, programType, type));
    // }
}