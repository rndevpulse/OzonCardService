
namespace OzonCard.BizClient.Models.Data
{
    public class Identification
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public override int GetHashCode()
        {
            return (Login + Password).GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType()) return false;
            var t = (Identification)obj;
            return (Login + Password) == (t.Login + t.Password);
        }
    }
}
