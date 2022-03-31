
namespace OzonCard.BizClient.Models.Data
{
    public class Session
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }


        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType()) return false;
            return Token == ((Session)obj).Token;
        }
    }
}
