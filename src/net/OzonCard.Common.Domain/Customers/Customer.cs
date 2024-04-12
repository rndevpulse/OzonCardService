using OzonCard.Common.Domain.Abstractions;
using OzonCard.Common.Domain.Cards;

namespace OzonCard.Common.Domain.Customers;

public class Customer : AggregateRoot
{
    private readonly ICollection<CustomerWallet> _wallets = new List<CustomerWallet>();

    
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string TabNumber { get; private set; }
    public string? Position { get; private set; }
    public string? Division { get; private set; }
    public bool IsActive { get; private set; }
    public string Comment { get; private set; }
    public Guid BizId { get; private set; }
    
    public List<Card> Cards { get; set; }
    public IEnumerable<CustomerWallet> Wallets => _wallets;

    public Customer(
        string name, string phone, 
        string tabNumber, 
        string? position, string? division, 
        bool isActive, string comment, Guid bizId, 
        List<Card> cards)
    {
        Name = name;
        Phone = phone;
        TabNumber = tabNumber;
        Position = position;
        Division = division;
        IsActive = isActive;
        Comment = comment;
        BizId = bizId;
        Cards = cards;
    }
}