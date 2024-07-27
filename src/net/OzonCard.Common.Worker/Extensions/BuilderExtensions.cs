using Hangfire;
using OzonCard.Common.Worker.Filters;
using OzonCard.Common.Worker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace OzonCard.Common.Worker.Extensions;

public static class BuilderExtensions
{
   
    public static IServiceCollection AddHangfireBackgroundJobService(
        this IServiceCollection services, 
        string connection, 
        string schema = "hangfire")
    {
        services.AddScoped<IBackgroundJobQueue, BackgroundJobQueue>();
        services.AddScoped<ITrackingBackgroundJobs, TrackingBackgroundJobsService>();
        services.AddScoped<IBackgroundJobsService, BackgroundJobService>();
   
        
        services.AddHangfire(hangfire =>
        {
            hangfire
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseFilter(new SkipWhenPreviousJobIsRunningAttribute())
                .UseFilter(new AutomaticRetryAttribute { Attempts = 3 })
                .UseSqlServerStorage(connection);
        });
       
        return services;
    }

    public static IServiceCollection AddHangfireServer(this IServiceCollection services) =>
        services.AddHangfireServer((_, options) =>
        {
            options.WorkerCount = Environment.ProcessorCount * 5;
            options.Queues = ["default", "recurring", "operational"];
        });
    

    private static string UseSchema(this string connectionString, string? schema)
    {
        if (string.IsNullOrEmpty(schema))
            schema = "hangfire";
        if (!connectionString.Contains("Search Path"))
            return connectionString + $"Search Path={schema};";
        return string.Join("", 
            connectionString
                .Split(';')
                .Where(x=>!x.Contains("Search Path"))
                .Select(x=>$"{x};")
            ) + $"Search Path={schema};"; 
    }
    
}