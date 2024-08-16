using Medallion.Threading;
using Medallion.Threading.SqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.Common.Application.Customers;
using OzonCard.Common.Application.Files;
using OzonCard.Common.Application.Organizations;
using OzonCard.Common.Application.Visits;
using OzonCard.Common.Core;
using OzonCard.Common.Infrastructure.Buses;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Database.Materialization;
using OzonCard.Common.Infrastructure.Piplines;
using OzonCard.Common.Infrastructure.Repositories;
using OzonCard.Common.Infrastructure.Services;
using OzonCard.Common.Worker.Extensions;
using OzonCard.Common.Worker.JobsProgress;

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
        
        #region Locks

        services.AddSingleton<IDistributedLockProvider>(_ =>
            new SqlDistributedSynchronizationProvider(options.Connection!)
        );

        #endregion
        return services;
    }
    
    
    private static IServiceCollection AddContext(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddDbContext<InfrastructureContext>(b =>
            (options.IsDevelopment ? b.EnableSensitiveDataLogging() : b).UseSqlServer(
                options.Connection,
                optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    optionsBuilder.UseCompatibilityLevel(120);
                })
            .AddInterceptors(ContextMaterializationInterceptor.Instance));
        services.AddDbContext<SecurityContext>(b =>
            (options.IsDevelopment ? b.EnableSensitiveDataLogging() : b).UseSqlServer(
                options.Connection));

        services.AddScoped<ITransactionManager>(sp => sp.GetRequiredService<InfrastructureContext>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));

        services.AddHostedService<DatabaseBootstrapService>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IJobProgressRepository, JobProgressRepository>();
        return services;

    }
    
    private static IServiceCollection AddMediatr(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(options.Assemblies));
        services.AddScoped<ICommandBus, MediatrCommandBus>();
        services.AddScoped<IQueryBus, MediatrQueryBus>();
        return services;
    }
    
    private static IServiceCollection AddHangfire(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddHangfireBackgroundJobService(options.Connection ?? string.Empty);

        if (!options.ServerWorker) 
            return services;
        services.AddHostedService<ScheduleUpdateBootstrapService>();
        services.AddHangfireServer();

        return services;
    }

}