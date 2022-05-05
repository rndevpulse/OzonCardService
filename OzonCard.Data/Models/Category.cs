
namespace OzonCard.Data.Models
{
    public class Category : EqualsId<Category>
    {
        override public Guid Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public List<CategoryCustomer> Customers { get; set; }

        public Category()
        {
            Customers = new List<CategoryCustomer>();
            Name = String.Empty;
        }
    }

    
}
