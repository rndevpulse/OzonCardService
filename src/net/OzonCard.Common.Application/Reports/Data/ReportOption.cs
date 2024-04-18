namespace OzonCard.Common.Application.Reports.Data;

public class ReportOption
{
    public Guid OrganizationId { get; set; }
    public IEnumerable<Guid> CategoriesId { get; set; } = new List<Guid>();
    public Guid CorporateNutritionId { get; set; }
    public string DateFrom { get; set; } = "";
    public string DateTo { get; set; } = "";
    public string Title { get; set; } = "";
    public bool IsOffline { get; set; }
    
    public Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
}