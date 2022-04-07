
namespace OzonCard.Data.Models
{
    public class Wallet : EqualsId<Wallet>
    {
        public string Name { get; set; }
        public string ProgramType { get; set; }
        public string Type { get; set; }

        public Wallet()
        {
            Name = String.Empty;
            ProgramType = String.Empty; 
            Type = String.Empty;
        }


        
    }
}
