
namespace OzonCard.Data.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? TabNumber { get; set; }
        public string? Position { get; set; }
        public bool IsActive { get; set; }
        public string? Comment { get; set; }
        public DateTime Create { get; set; }
        public Guid iikoBizId { get; set; }

        public Organization? Organization { get; set; }
        public IEnumerable<Card>? Cards { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<CustomerWallet>? Wallets { get; set; }

    }
}
