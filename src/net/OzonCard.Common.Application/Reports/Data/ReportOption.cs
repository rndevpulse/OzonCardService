using OzonCard.Common.Application.Common;

namespace OzonCard.Common.Application.Reports.Data;

public class ReportOption : MemberInfo
{
    public Guid OrganizationId { get; set; }
    public IEnumerable<Guid> CategoriesId { get; set; } = new List<Guid>();
    public Guid ProgramId { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public int Offset { get; set; }
    public string Title { get; set; } = "";
    public bool IsOffline { get; set; }
    
}