using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonCard.Common.Application.Customers.Commands;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Worker.Services;

namespace OzonCard.Common.Infrastructure.Services;

public class ScheduleUpdateBootstrapService(IServiceProvider provider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = provider.CreateScope();

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
       
        return Task.CompletedTask;

    }
}