using OzonCard.Customer.Api.Models.Organizations;

namespace OzonCard.Customer.Api.Models.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    public Guid BizId { get; set; }
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
    public string TabNumber { get; set; } = "";
    public string Position { get; set; } = "";
    public string Division { get; set; } = "";
    public string Organization { get; set; } = "";
    public decimal? Balance { get; set; }
    public decimal? Sum { get; set; }
    public decimal? Orders { get; set; }
    public IEnumerable<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

    public DateTimeOffset? LastVisit { get; set; }
    public DateTimeOffset? Updated { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public int? DaysGrant { get; set; }
}