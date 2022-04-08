
namespace OzonCard.BizClient.Models
{
    public class ReportCN
    {
        public int balanceOnPeriodEnd { get; set; }
        public int balanceOnPeriodStart { get; set; }
        public int balanceRefillSum { get; set; }
        public int balanceResetSum { get; set; }
        public string employeeNumber { get; set; }
        public string guestCardTrack { get; set; }
        public string guestCategoryNames { get; set; }
        public Guid guestId { get; set; }
        public string guestName { get; set; }
        public string guestPhone { get; set; }
        public int paidOrdersCount { get; set; }
        public int payFromWalletSum { get; set; }
    }
}
