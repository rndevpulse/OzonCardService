
namespace OzonCard.Data.Models
{
    public class CategoryCustomer
    {
        public Guid CategoryId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Category Category { get; set; }

    }

    
}
