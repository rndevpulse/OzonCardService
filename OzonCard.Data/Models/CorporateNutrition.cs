
namespace OzonCard.Data.Models
{
    public class CorporateNutrition
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Wallet>? Wallets { get; set; }
    }
}
