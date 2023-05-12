namespace OzonCard.Data.Models;

public class CustomerTransactions
{
    public string Card { get; }
    public DateTime LastVisit { get; }
    public IEnumerable<DateTime> Transactions { get; }
    public int DaysGrant => Transactions.GroupBy(x => x.Date).Count();

    public CustomerTransactions(string card, DateTime lastVisit, IEnumerable<DateTime> transactions)
    {
        Card = card;
        LastVisit = lastVisit;
        Transactions = transactions;
    }
}