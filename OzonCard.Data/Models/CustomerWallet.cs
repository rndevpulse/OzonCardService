
namespace OzonCard.Data.Models
{
    public class CustomerWallet
    {
        public Guid Id { get; set; }
        public double Balance { get; set; }
        public Wallet? Wallet { get; set; }
        public DateTime Update { get; set; }
    }
}
