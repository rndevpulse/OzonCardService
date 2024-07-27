using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Reports.Data;

public record ReportsTaskProgress : NamedProgress<ReportsTaskProgress>
{
    public string Description { get; set; } = "";
    public int Progress { get; set; }
    public override void Report(ReportsTaskProgress value)
    {
        Description = value.Description;
        Progress = value.Progress;
    }
}