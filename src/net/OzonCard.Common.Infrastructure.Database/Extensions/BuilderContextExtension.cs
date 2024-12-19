using Medallion.Threading;
using Medallion.Threading.Postgres;
using Medallion.Threading.SqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.Common.Infrastructure.Database.Contexts;
using OzonCard.Common.Infrastructure.Database.Materialization;
using OzonCard.Common.Infrastructure.Database.Pipelines;
using OzonCard.Common.Infrastructure.Database.Services;

namespace OzonCard.Common.Infrastructure.Database.Extensions;

public static class BuilderContextExtension
{
    public static IServiceCollection AddContext(this IServiceCollection services, InfrastructureDatabaseOptions options)
    {
       
        switch (options.Provider)
        {
            case "sqlserver":
                services.AddSingleton<IDistributedLockProvider>(_ =>
                    new SqlDistributedSynchronizationProvider(options.Connection!)
                );
                break;
            case "postgres":
                services.AddSingleton<IDistributedLockProvider>(_ =>
                    new PostgresDistributedSynchronizationProvider(options.Connection!)
                );
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(options.Provider), options.Provider,
                    $"Unsupported database provider {options.Provider}");
        }
        
        services.AddDbContext<InfrastructureContext>(b =>
            b.ConfigureContext(options)
                .AddInterceptors(ContextMaterializationInterceptor.Instance)
            );
        services.AddDbContext<SecurityContext>(b =>
            b.ConfigureContext(options));
        services.AddDbContext<TaskContext>(b =>
            b.ConfigureContext(options), ServiceLifetime.Transient);

        services.AddScoped<ITransactionManager>(sp => sp.GetRequiredService<InfrastructureContext>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));
        
        services.AddHostedService<DatabaseBootstrapService>();
        
        return services;
    }
}