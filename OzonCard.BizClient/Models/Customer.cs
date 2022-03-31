
namespace OzonCard.BizClient.Models
{
    public class Customer
    {
        public bool anonymized { get; set; }
        public object birthday { get; set; }
        public Card[] cards { get; set; }
        public Category[] categories { get; set; }
        public string comment { get; set; }
        public int consentStatus { get; set; }
        public string cultureName { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public int iikoCardOrdersSum { get; set; }
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public string middleName { get; set; }
        public string name { get; set; }
        public object personalDataConsentFrom { get; set; }
        public object personalDataConsentTo { get; set; }
        public object personalDataProcessingFrom { get; set; }
        public object personalDataProcessingTo { get; set; }
        public string phone { get; set; }
        public string rank { get; set; }
        public object referrerId { get; set; }
        public int sex { get; set; }
        public bool shouldReceiveLoyaltyInfo { get; set; }
        public bool shouldReceiveOrderStatusInfo { get; set; }
        public bool shouldReceivePromoActionsInfo { get; set; }
        public string surname { get; set; }
        public string userData { get; set; }
        public Walletbalance[] walletBalances { get; set; }
    }
}
