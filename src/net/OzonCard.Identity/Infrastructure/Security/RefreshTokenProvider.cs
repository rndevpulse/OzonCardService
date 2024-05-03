using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OzonCard.Identity.Infrastructure.Security;

public class RefreshTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : IdentityUser
{
    public RefreshTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
    {
    }

}