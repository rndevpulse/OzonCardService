﻿
namespace OzonCard.Data.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
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
