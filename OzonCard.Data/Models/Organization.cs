
namespace OzonCard.Data.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Category> Categories { get; set; }
        public List<CorporateNutrition> CorporateNutritions { get; set; }
        public List<User> Users { get; set; }

        public Organization()
        {
            Name = String.Empty;
            Password = String.Empty;
            Categories = new List<Category>();
            CorporateNutritions = new List<CorporateNutrition>();
            Users = new List<User>();
            Login = String.Empty;
        }

    }
}
