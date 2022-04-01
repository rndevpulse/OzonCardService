
namespace OzonCard.BizClient.Models
{
    public class CorporateNutrition
    {
        public object description { get; set; }
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime? serviceFrom { get; set; }
        public DateTime? serviceTo { get; set; }
        public Wallet[] wallets { get; set; }
    }
}
