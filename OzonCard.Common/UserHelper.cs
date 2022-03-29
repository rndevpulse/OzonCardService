using System.Security.Cryptography;
using System.Text;

namespace OzonCard.Common
{
    public static class UserHelper
    {
        static public string NewKeyActivation()
        {
            var random = new Random();
            var suffix = "1234567890";
            return new string(
                Enumerable.Repeat(suffix, 6)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static public Guid GetHash(string pass)
        {
            byte[] bts = Encoding.Unicode.GetBytes(pass);
            var md5 = MD5.Create();
            byte[] btsHash = md5.ComputeHash(bts);
            string hash = string.Empty;
            foreach (byte b in btsHash)
                hash += string.Format("{0:x2}", b);
            return new Guid(hash);
        }
    }
}
