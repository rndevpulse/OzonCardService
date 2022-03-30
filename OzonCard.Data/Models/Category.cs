
namespace OzonCard.Data.Models
{
    public class Category : EqualsId<Category>
    {
        override public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Customer> Customers { get; set; }

        public Category()
        {
            Customers = new List<Customer>();
            Name = String.Empty;
        }
    }

    
}
