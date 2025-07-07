using Microsoft.AspNetCore.Identity;

namespace OzonCard.Identity.Infrastructure.Security;

public static class RefreshTokenProviderExtensions
{
    public static IdentityBuilder AddRefreshTokenProvider<TUser>(this IdentityBuilder builder)
        where TUser : IdentityUser =>
        builder.AddTokenProvider<RefreshTokenProvider<TUser>>(RefreshTokenProviderOptions.ProviderName);

    public static async Task<string> GenerateRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user)
        where TUser : class
    {
        var value =  await userManager.GenerateUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, "");
        var token = Guid.NewGuid().ToString("N");
        await userManager.SetAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName, token, value);
        return token;
    }

    public static async Task<bool> VerifyRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string token) where TUser : class
    {
        var value = await userManager.GetAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName, token);
        return await userManager.VerifyUserTokenAsync(user, RefreshTokenProviderOptions.ProviderName, "", value ?? "");
    }
   
    public static Task<IdentityResult> RemoveRefreshTokenAsync<TUser>(this UserManager<TUser> userManager,
        TUser user, string token) where TUser : class =>
        userManager.RemoveAuthenticationTokenAsync(user, RefreshTokenProviderOptions.ProviderName, token);
}