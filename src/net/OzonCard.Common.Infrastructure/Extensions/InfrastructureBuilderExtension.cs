using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Properties;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Core;
using OzonCard.Common.Infrastructure.Buses;
using OzonCard.Common.Infrastructure.Database.Contexts;
using OzonCard.Common.Infrastructure.Database.Extensions;
using OzonCard.Common.Infrastructure.Repositories;
using OzonCard.Common.Infrastructure.Services;
using OzonCard.Common.Infrastructure.Stores;
using OzonCard.Common.Worker.Extensions;

namespace OzonCard.Common.Infrastructure.Extensions;



public static class InfrastructureBuilderExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<InfrastructureOptions> configureOptions)
    {
        var options = new InfrastructureOptions();
        configureOptions.Invoke(options);
        services.AddContext(options);
        services.AddMediatr(options);
        services.AddRepositories(options);
        services.AddHangfire(options);
        
       
        return services;
    }
    
    
    

    private static IServiceCollection AddRepositories(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IPropertiesRepository, PropertiesRepository>();
        
        services.AddScoped<IStoreContext>(sp=> 
            new StoreTaskContext(
                new DbContextOptionsBuilder<TaskContext>()
                    .ConfigureContext(options)
                    .Options,
                sp.GetRequiredService<ILogger<StoreTaskContext>>()));

        return services;

    }
    
    private static IServiceCollection AddMediatr(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddMediatR(configuration =>  configuration.RegisterServicesFromAssemblies(options.Assemblies));
        services.AddScoped<ICommandBus, MediatrCommandBus>();
        services.AddScoped<IQueryBus, MediatrQueryBus>();
        services.AddScoped<IEventBus, MediatrEventBus>();
        return services;
    }
    
    private static IServiceCollection AddHangfire(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddHangfireBackgroundJobService(options.Connection ?? string.Empty, options.Provider);

        if (!options.ServerWorker) 
            return services;
        services.AddHostedService<ScheduleUpdateBootstrapService>();
        services.AddHangfireServer();

        return services;
    }

}