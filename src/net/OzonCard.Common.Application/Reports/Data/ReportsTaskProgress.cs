using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Reports.Data;

public record ReportsTaskProgress : NamedProgress
{
    public string Description { get; set; } = "";
    public int Progress { get; set; }
   
}