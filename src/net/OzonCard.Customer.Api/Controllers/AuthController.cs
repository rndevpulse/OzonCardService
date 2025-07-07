using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.Auth;
using OzonCard.Identity.Application.Authenticate.Commands;

namespace OzonCard.Customer.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{

    [HttpGet("[action]"), Authorize]
    public IActionResult Check(CancellationToken ct = default) => new OkResult();

    
    
    [HttpPost("[action]")]
    public async Task<AuthTokenModel> Login(LoginModel model, CancellationToken ct = default)
    {
        var auth = await Commands.Send(new SigInCommand(model.Email, model.Password), ct);
        // SetTokenCookie(auth.Refresh);
        return new AuthTokenModel(auth.Access, auth.Refresh, auth.Roles);
    }

    [HttpGet("[action]")]
    public async Task<AuthTokenModel> Refresh([FromQuery] string token, CancellationToken ct = default)
    {
        var auth = await Commands.Send(
            new UpdateRefreshTokenCommand(
                UserClaimSid.ToString(),
                token
            ), ct);
        return new AuthTokenModel(auth.Access, auth.Refresh, auth.Roles);
    }


    [HttpGet("[action]")]
    public async Task Logout(CancellationToken ct = default)
    {
        if (UserClaimSid is {} userId)
            await Commands.Send(new LogoutCommand(userId.ToString()), ct);
    }
    
}