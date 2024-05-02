using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;

namespace OzonCard.Common.Application.Customers.Queries;

public class CustomersSearchQuery : IQuery<IEnumerable<CustomerSearch>>
{
    public string Name { get; set; } = "";
    public string Card { get; set; } = "";
    public Guid OrganizationId { get; set; }
    public Guid ProgramId { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public int Offset { get; set; }

}