using Microsoft.AspNetCore.Authorization;
using OzonCard.Data.Enums;
using System.Linq;

namespace OzonCardService.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params EnumRules[] roles) : base()
        {
            Roles = string.Join(',', roles.Select(x => (int)x));
        }
        
    }
}
