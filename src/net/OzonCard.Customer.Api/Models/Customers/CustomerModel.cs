namespace OzonCard.Customer.Api.Models.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    public Guid BizId { get; set; }
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
    public string TabNumber { get; set; } = "";
    public string Position { get; set; } = "";
    public string Division { get; set; } = "";
    public string Organization { get; set; } = "";
    public decimal? Balance { get; set; }
    public decimal? Sum { get; set; }
    public decimal? Orders { get; set; }
    public IEnumerable<string> Categories { get; set; } = new List<string>();

    public DateTime LastVisit { get; set; }
    public int? DaysGrant { get; set; }
}