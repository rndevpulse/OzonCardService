using OzonCard.Common.Application.Properties.Data;

namespace OzonCard.Customer.Api.Models.Props;

public class ReportBatchModel
{
    public Guid Organization { get; set; }
    public string Name { get; set; }
    public IEnumerable<ReportBatchProp> Properties { get; set; }
}