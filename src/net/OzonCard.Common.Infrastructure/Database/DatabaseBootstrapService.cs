using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OzonCard.Common.Infrastructure.Database;

public class DatabaseBootstrapService : BackgroundService
{
    private readonly IServiceProvider _rootProvider;
    private readonly ILogger<DatabaseBootstrapService> _logger;

    public DatabaseBootstrapService(IServiceProvider rootProvider, ILogger<DatabaseBootstrapService> logger)
    {
        _rootProvider = rootProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _rootProvider.CreateScope();
        var provider = scope.ServiceProvider;

        await MigrateContextAsync(provider.GetRequiredService<SecurityContext>(), stoppingToken);
        await MigrateContextAsync(provider.GetRequiredService<InfrastructureContext>(), stoppingToken);
    }

    private async Task MigrateContextAsync(DbContext context, CancellationToken ct)
    {
        var contextName = context.GetType().Name;
        var migrations = await context.Database.GetPendingMigrationsAsync(ct);

        if (migrations.Any())
            _logger.LogInformation("Applying migrations {Migrations} for context {Content} ",
                string.Join(',', migrations),
                contextName
            );

        await context.Database.MigrateAsync(ct);
    }
}
