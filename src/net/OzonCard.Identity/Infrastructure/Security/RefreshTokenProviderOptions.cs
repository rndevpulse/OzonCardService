using Microsoft.AspNetCore.Identity;

namespace OzonCard.Identity.Infrastructure.Security;

public class RefreshTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public const string ProviderName = "RefreshTokenProvider";
    public const string TokenName = "RefreshToken";

    public RefreshTokenProviderOptions()
    {
        Name = ProviderName;
                
        TokenLifespan = TimeSpan.FromDays(20);
    }
}