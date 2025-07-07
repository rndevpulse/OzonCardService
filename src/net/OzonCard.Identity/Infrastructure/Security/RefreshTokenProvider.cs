using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OzonCard.Identity.Infrastructure.Security;

public class RefreshTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : IdentityUser
{

    public RefreshTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
    {
        options.Value.TokenLifespan = TimeSpan.FromDays(7);
    }

    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var token = await base.GenerateAsync(purpose, manager, user);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
    }

    public override Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        var value = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        return base.ValidateAsync(purpose, value, manager, user);
    }
}