
namespace OzonCard.Data.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<CorporateNutrition>? CorporateNutritions { get; set; }
        public IEnumerable<User>? Users { get; set; }

    }
}
