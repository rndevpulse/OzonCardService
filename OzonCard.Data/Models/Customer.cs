
namespace OzonCard.Data.Models
{
    public class Customer : EqualsId<Customer>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TabNumber { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }
        public string Comment { get; set; }
        public DateTime Create { get; set; }
        public Guid iikoBizId { get; set; }

        public Organization Organization { get; set; }
        public List<Card> Cards { get; set; }
        public List<CategoryCustomer> Categories { get; set; }
        public List<CustomerWallet> Wallets { get; set; }


        public Customer()
        {
            Name = String.Empty;
            Phone = String.Empty;
            TabNumber = String.Empty;
            Position = String.Empty;
            Comment = String.Empty;
            Cards = new List<Card>();
            Categories = new List<CategoryCustomer>();
            Wallets = new List<CustomerWallet>();
            Organization = new Organization();

            Create = DateTime.UtcNow;
            IsActive = true;
        }

    }
}
