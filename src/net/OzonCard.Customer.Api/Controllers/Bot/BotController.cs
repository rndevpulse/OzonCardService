using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.Common.Core;

namespace OzonCard.Customer.Api.Controllers.Bot;

[Authorize(Policy = "ApiKeyPolicy")]
[ApiController]
[ApiVersion("1.0")]
[Route("bot/v{version:apiVersion}/[controller]")]
public abstract class BotController : Controller
{
    protected IQueryBus Queries => HttpContext.RequestServices.GetRequiredService<IQueryBus>();
    protected ICommandBus Commands => HttpContext.RequestServices.GetRequiredService<ICommandBus>();
    protected IMapper Mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();
}