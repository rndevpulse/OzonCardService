using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.Auth;
using OzonCard.Customer.Api.Models.Users;
using OzonCard.Identity.Application.Authenticate.Commands;
using OzonCard.Identity.Application.Users.Commands;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

public class AuthController : ApiController
{

    [HttpPost("[action]"), Authorize(UserRole.Admin)]
    public async Task<UserModel> Create(CreateUserModel model, CancellationToken ct = default)
    {
        var user = await Commands.Send(
            new CreateUserCommand(model.Email, model.Password, model.Roles),
            ct);
        return new UserModel(
            Guid.Parse(user.Id), model.Email, ArraySegment<UserOrganizationModel>.Empty);
    }
    
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