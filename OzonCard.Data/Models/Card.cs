

namespace OzonCard.Data.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Track { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
        public DateTime Create { get; set; }

        public Card(string track)
        {
            Track = track;
            Number = track;
            IsActive = true;
            Create = DateTime.UtcNow;
        }
        public Card() {
            Track = String.Empty;
            Number = String.Empty;
        }

    }
}
