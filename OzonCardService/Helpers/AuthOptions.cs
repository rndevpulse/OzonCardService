using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OzonCardService.Helpers
{
    public class AuthOptions
    {
        public const string ISSUER = "PServer"; // издатель токена
        public const string AUDIENCE = "PClient"; // потребитель токена
        const string KEY = "bd&'/}Z64c-($>2j:Zee0f158bX-BGftaVk`)9de084t*kW'7f8";   // ключ для шифрации
        public const int LIFETIME = 300; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
