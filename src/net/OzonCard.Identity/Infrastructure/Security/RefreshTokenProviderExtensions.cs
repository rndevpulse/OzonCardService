using Microsoft.AspNetCore.Identity;

namespace OzonCard.Identity.Infrastructure.Security;

public static class RefreshTokenProviderExtensions
{
    public static IdentityBuilder AddRefreshTokenProvider<TUser>(this IdentityBuilder builder)
        where TUser : IdentityUser =>
        builder.AddTokenProvider<RefreshTokenProvider<TUser>>(RefreshTokenProviderOptions.ProviderName);
    
    public static Task<string> GenerateRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user)
        where TUser : class =>
        userManager.GenerateUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, "");

    public static Task<bool> VerifyRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string token) where TUser : class =>
        userManager.VerifyUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, "", token);

    public static Task<IdentityResult> SetRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string tokenValue) where TUser : class =>
        userManager.SetAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName,
            // Guid.NewGuid().ToString("N"),
            "refresh",
            tokenValue);


    public static Task<IdentityResult> RemoveRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user) where TUser : class =>
        userManager.RemoveAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName, "refresh");
}