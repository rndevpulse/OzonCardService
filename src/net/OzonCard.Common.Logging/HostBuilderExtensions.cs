using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace OzonCard.Common.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseDefaultLogging(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((hbContext, lConfig) => lConfig.ReadFrom.Configuration(hbContext.Configuration));
    
    public static WebApplicationBuilder UseDefaultLogging(this WebApplicationBuilder hostBuilder)
    {
        hostBuilder.Host.UseDefaultLogging();
        return hostBuilder;
    }

    public static IHostApplicationBuilder UseDefaultLogging(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSerilog(lConfig => lConfig.ReadFrom.Configuration(builder.Configuration));
        return builder;
    }

  
}