
namespace OzonCard.Data.Models
{
    public class CorporateNutrition : EqualsId<CorporateNutrition>
    {
        override public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Wallet> Wallets { get; set; }

        public CorporateNutrition()
        {
            Name = String.Empty;
            Description = String.Empty;
            Wallets = new List<Wallet>();
        }

    }
}
