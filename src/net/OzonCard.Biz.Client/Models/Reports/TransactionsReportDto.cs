namespace OzonCard.Biz.Client.Models.Reports;

public class TransactionsReportDto
{
    public string TransactionCreateDate { get; set; }  = "";

    public DateTime CreateDate => DateTime.Parse(TransactionCreateDate);
    // public DateTime OrderCreateDate { get; set; }
    public long? OrderNumber { get; set; }
    public decimal? TransactionSum { get; set; }
    public string TransactionType { get; set; } = "";
    public decimal? OrderSum { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CardNumbers { get; set; }
    public string? Comment { get; set; }
    public Guid OrganizationId { get; set; }
    public string? ProgramName { get; set; }
    public string? MarketingCampaignName { get; set; }
}