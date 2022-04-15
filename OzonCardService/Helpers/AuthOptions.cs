using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OzonCardService.Helpers
{
    public class AuthOptions
    {
        public const string ISSUER = "OzonCardServer"; // издатель токена
        public const string AUDIENCE = "OzonCardClient"; // потребитель токена
        const string KEY = "bd&'/}Z64c-($>2j:Zee0f158bX-BasdAErstaVk`)9de084t*kW'7f8";   // ключ для шифрации
        public const int LIFETIME = 15; // время жизни токена - минут
        public const int LIFETIME_REFRESH = 7; // время жизни токена - дней
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
