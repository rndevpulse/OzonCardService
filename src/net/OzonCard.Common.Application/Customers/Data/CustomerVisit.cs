namespace OzonCard.Common.Application.Customers.Data;

public class CustomerVisit
{
    public string? Card { get; }
    private List<DateTimeOffset> Visits { get; }
    public DateTimeOffset LastVisitDate {get;}
    public int DaysGrant {get;}
    public CustomerVisit(string? card, IEnumerable<DateTimeOffset> visits)
    {
        Card = card;
        Visits = visits.ToList();
        LastVisitDate = Visits.Max(r => r);
        DaysGrant = Visits.GroupBy(t => t).Count();

    }
}