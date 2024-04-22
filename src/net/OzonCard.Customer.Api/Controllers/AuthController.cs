using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Customer.Api.Models.Auth;
using OzonCard.Identity.Application.Authenticate.Commands;

namespace OzonCard.Customer.Api.Controllers;

public class AuthController : ApiController
{

    
    
    [HttpPost("[action]"), AllowAnonymous]
    public async Task<AuthTokenModel> Login(LoginModel model, CancellationToken ct = default)
    {
        var auth = await Commands.Send(new SigInCommand(model.Email, model.Password), ct);
        SetTokenCookie(auth.Refresh);
        return new AuthTokenModel(auth.Access, auth.Refresh, auth.Roles);
    }
    
    [HttpGet("[action]")]
    public async Task<AuthTokenModel> Refresh(CancellationToken ct = default)
    {
        var token = await Response.HttpContext.GetTokenAsync("access_token")
            ?? throw new BusinessException("Token is corrupted");
        var auth = await Commands.Send(
            new UpdateRefreshTokenCommand(
                token,
                Request.Cookies["refreshToken"] ?? ""
            ), ct);
        SetTokenCookie(auth.Refresh);
        return new AuthTokenModel(auth.Access, auth.Refresh, auth.Roles);
    }
    

    [HttpGet("[action]"), AllowAnonymous]
    public async Task Logout(CancellationToken ct = default)
    {
        var token = await Response.HttpContext.GetTokenAsync("access_token");
        var t = UserClaimEmail;
        if (token == null)
            return;
        await Commands.Send(
            new LogoutCommand(
                token,
                Request.Cookies["refreshToken"] ?? ""
            ), ct);
        Response.Cookies.Delete("refreshToken");
    }
    

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions()
        {
            SameSite = SameSiteMode.None,
            Secure = true,
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(20)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}