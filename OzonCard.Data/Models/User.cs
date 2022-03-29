
using OzonCard.Data.Enums;

namespace OzonCard.Data.Models
{
    public  class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Mail { get; set; }
        public Guid Password { get; set; }
        public IEnumerable<Organization>? Organizations { get; set; }
        public string? Rules { get; set; }
        public DateTime? CreatedDate { get; set; }


    }
}
