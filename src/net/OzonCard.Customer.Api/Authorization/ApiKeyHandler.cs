using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OzonCard.Customer.Api.Authorization;

public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _key;
    public readonly string Agent = "BotRestEase";
    
    
    public ApiKeyHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _key = configuration.GetValue<string>("apiKey");
    }
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        var apiKey = _httpContextAccessor.HttpContext?.Request.Headers["X-Api-Key"].ToString();
        var agent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
        if (!Agent.Equals(agent) || string.IsNullOrWhiteSpace(apiKey) || !_key.Equals(apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}