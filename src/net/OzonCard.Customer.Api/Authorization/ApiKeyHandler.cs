using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OzonCard.Customer.Api.Authorization;

public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static string? _key = null;
    private static readonly string _keyHeader = "X-Api-Key";
    
    public ApiKeyHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _key ??= configuration.GetValue<string>("apiKey");
    }
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        var apiKey = _httpContextAccessor?.HttpContext?.Request.Headers[_keyHeader].ToString();
        if (string.IsNullOrWhiteSpace(apiKey) || !_key.Equals(apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}