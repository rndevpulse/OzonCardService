namespace OzonCard.Excel
{
    public class ShortCustomerInfo_excel
    {
        public string TabNumber { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public string Position { get; set; }

        public ShortCustomerInfo_excel()
        {
            TabNumber = String.Empty;
            Name = String.Empty;
            Card = String.Empty;
            Position = String.Empty;
        }
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType()) return false;
            return Card == ((ShortCustomerInfo_excel)obj).Card;
        }
        public override int GetHashCode()
        {
            return Card.GetHashCode();
        }

    }
}
