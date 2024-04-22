using Microsoft.AspNetCore.Identity;

namespace OzonCard.Identity.Infrastructure.Security;

public static class RefreshTokenProviderExtensions
{
    public static IdentityBuilder AddRefreshTokenProvider<TUser>(this IdentityBuilder builder,
        string name = RefreshTokenProviderOptions.ProviderName)
        where TUser : IdentityUser =>
        builder.AddTokenProvider<RefreshTokenProvider<TUser>>(name);
    
    public static Task<string> GenerateRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string purpose)
        where TUser : class =>
        userManager.GenerateUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, purpose);

    public static Task<bool> VerifyRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string token, string purpose) where TUser : class =>
        userManager.VerifyUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, purpose, token);

    public static Task<IdentityResult> SetRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string tokenValue) where TUser : class =>
        userManager.SetAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName,
            Guid.NewGuid().ToString("N"), tokenValue);


    public static Task<IdentityResult> RemoveRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string tokenName) where TUser : class =>
        userManager.RemoveAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName, tokenName);
}