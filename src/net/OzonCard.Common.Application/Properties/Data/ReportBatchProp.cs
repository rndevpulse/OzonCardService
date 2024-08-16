namespace OzonCard.Common.Application.Properties.Data;

public class ReportBatchProp
{
    public string Name { get; set; }
    public IEnumerable<Guid> Aggregations { get; set; }
}
