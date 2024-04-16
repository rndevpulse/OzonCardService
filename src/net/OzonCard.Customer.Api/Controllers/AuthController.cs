using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.Auth;
using OzonCard.Identity.Authenticate.Commands;

namespace OzonCard.Customer.Api.Controllers;

public class AuthController : ApiController
{

    [HttpPost, AllowAnonymous]
    public async Task<AuthTokenModel> Token(AuthLoginModel model, CancellationToken ct = default)
    {
        var auth = await Commands.Send(new SigInCommand(model.Email, model.Password), ct);
        SetTokenCookie(auth.Refresh);
        return new AuthTokenModel(auth.Access, auth.Rules);
    }

    [HttpPost("[action]"), AllowAnonymous]
    public async Task Logout(string? refresh, CancellationToken ct = default)
    {
        var refreshToken = refresh ?? Request.Cookies["refreshToken"];
        await Commands.Send(new LogoutCommand(refreshToken), ct);
        Response.Cookies.Delete("refreshToken");

    }

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions()
        {
            SameSite = SameSiteMode.None,
            Secure = true,
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}