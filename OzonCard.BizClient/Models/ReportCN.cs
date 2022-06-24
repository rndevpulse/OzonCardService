
namespace OzonCard.BizClient.Models
{
    public class ReportCN
    {
        public double balanceOnPeriodEnd { get; set; }
        public double balanceOnPeriodStart { get; set; }
        public double balanceRefillSum { get; set; }
        public double balanceResetSum { get; set; }
        public string employeeNumber { get; set; }
        public string guestCardTrack { get; set; }
        public string guestCategoryNames { get; set; }
        public Guid guestId { get; set; }
        public string guestName { get; set; }
        public string guestPhone { get; set; }
        public double paidOrdersCount { get; set; }
        public double payFromWalletSum { get; set; }
    }
}
