

namespace OzonCard.BizClient.Models
{
    public class TransactionsReport
    {
        public DateTime transactionCreateDate { get; set; }
        public DateTime? orderCreateDate { get; set; }
        public long? orderNumber { get; set; }
        public double? transactionSum { get; set; }
        public string? transactionType { get; set; }
        public double? orderSum { get; set; }
        public string? phoneNumber { get; set; }
        public string? cardNumbers { get; set; }
        public string? comment { get; set; }
        public Guid organizationId { get; set; }
        public string? programName { get; set; }
        public string? marketingCampaignName { get; set; }
    }
}
