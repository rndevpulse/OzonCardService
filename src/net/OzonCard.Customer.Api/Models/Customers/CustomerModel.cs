namespace OzonCard.Customer.Api.Models.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
    public string TabNumber { get; set; } = "";
    public string Organization { get; set; } = "";
    public decimal Balance { get; set; }
    public DateTime? TimeUpdateBalance { get; set; }
    public decimal Sum { get; set; }
    public int Orders { get; set; }
    public IEnumerable<string> Categories { get; set; } = new List<string>();

    public string LastVisit { get; set; }  = "";
    public int DaysGrant { get; set; }
}