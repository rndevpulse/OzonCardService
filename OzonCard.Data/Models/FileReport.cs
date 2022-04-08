
namespace OzonCard.Data.Models
{
    public class FileReport
    {
        public Guid Id { get; set; }
        public string Format { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }

        public FileReport()
        {
            Format = string.Empty;
            Name = string.Empty;
            Created = DateTime.UtcNow;
        }
    }
}
