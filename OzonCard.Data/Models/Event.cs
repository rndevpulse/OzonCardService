
namespace OzonCard.Data.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime Create { get; set; }
        public DateTime? OrderCreate { get; set; }
        public long OrderNumber { get; set; }
        public double TransactionSum { get; set; }
        public string TransactionType { get; set; }
        public double OrderSum { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumbers { get; set; }
        public string Comment { get; set; }
        public string ProgramName { get; set; }
        public string MarketingCampaignName { get; set; }
        public Event()
        {
            TransactionType = string.Empty;
            PhoneNumber = string.Empty;
            CardNumbers = string.Empty;
            Comment = string.Empty;
            ProgramName = string.Empty;
            MarketingCampaignName = string.Empty;
        }
    }
}
