

namespace OzonCard.Data.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string? Track { get; set; }
        public string? Number { get; set; }
        public bool IsActive { get; set; }
        public DateTime Create { get; set; }
    }
}
