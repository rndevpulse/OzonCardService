using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.Common.Core;

namespace OzonCard.Customer.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : Controller
{
    protected IQueryBus Queries => HttpContext.RequestServices.GetRequiredService<IQueryBus>();
    protected ICommandBus Commands => HttpContext.RequestServices.GetRequiredService<ICommandBus>();
    protected IMapper Mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected Guid UserClaimSid => Guid.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var sid)
        ? sid
        : Guid.Empty;

    protected string? UserClaimEmail => User.FindFirstValue(ClaimTypes.Email);
}