using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Worker.Services;

namespace OzonCard.Common.Infrastructure.Services;

public class MyBackgroundJob
{
    public string Run()
    {
        // Job logic with a return value
        return "123 Job completed successfullyб уифтн цкще.";
    }
}

public class ScheduleUpdateBootstrapService(IServiceProvider provider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = provider.CreateScope();

        var q = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
        var repository = scope.ServiceProvider.GetRequiredService<IOrganizationRepository>();
        var jobsService = scope.ServiceProvider.GetRequiredService<IBackgroundJobsService>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        
        var days = configuration.GetValue("jobs:period", 1);
        var queue = "recurring";
        foreach (var org in repository.GetQuery())
        {
            var token = $"CUSTOMER_VISITS_${org.Id}";
            var command = new CustomersVisitsFetchCommand(org.Id, days);
            
            jobsService.Dequeue(token);
            
            jobsService.AppendSchedule(token, command, "*/30 * * * *", queue);
        }

        var t = q.Enqueue<MyBackgroundJob>(x => x.Run());
        q.ContinueJobWith<MyBackgroundJob>(t, x=> x.Run());
        return Task.CompletedTask;

    }
}