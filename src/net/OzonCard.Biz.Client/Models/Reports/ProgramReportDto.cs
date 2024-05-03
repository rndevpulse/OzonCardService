namespace OzonCard.Biz.Client.Models.Reports;

public class ProgramReportDto
{
    public decimal BalanceOnPeriodEnd { get; set; }
    public decimal BalanceOnPeriodStart { get; set; }
    public decimal BlanceRefillSum { get; set; }
    public decimal BalanceResetSum { get; set; }
    public string EmployeeNumber { get; set; } = "";
    public string GuestCardTrack { get; set; } = "";
    public string GuestCategoryNames { get; set; } = "";
    public Guid GuestId { get; set; }
    public string GuestName { get; set; } = "";
    public string GuestPhone { get; set; } = "";
    public decimal PaidOrdersCount { get; set; }
    public decimal PayFromWalletSum { get; set; }
}