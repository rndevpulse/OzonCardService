
using OzonCard.Data.Enums;

namespace OzonCard.Data.Models
{
    public  class User :  EqualsId<User>
    {
        override public Guid Id { get; set; }
        public string Mail { get; set; }
        public Guid Password { get; set; }
        public List<Organization> Organizations { get; set; }
        public string Rules { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            Mail = String.Empty;
            Organizations = new List<Organization>();
            Rules = String.Empty;
            CreatedDate = DateTime.UtcNow;
            RefreshTokens = new List<RefreshToken>();
        }

    }
}
