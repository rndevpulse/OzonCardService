using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Infrastructure.Database.Contexts;

namespace OzonCard.Common.Infrastructure.Database.Extensions;

public static class ContextConfigurationExtension
{
    public static DbContextOptionsBuilder ConfigureContext(
        this DbContextOptionsBuilder builder, 
        InfrastructureDatabaseOptions options) =>
        options.Provider switch
        {
            "sqlserver" => builder.AddSqlServerDatabase(options.Connection, options.IsDevelopment),
            "postgres" => builder.AddPostgresDatabase(options.Connection, options.IsDevelopment),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Provider), options.Provider,
                $"Unsupported database provider {options.Provider}")
        };

    public static DbContextOptionsBuilder ConfigureContext(
        this DbContextOptionsBuilder builder, 
        string connection,
        string provider) =>
        provider switch
        {
            "sqlserver" => builder.AddSqlServerDatabase(connection,false),
            "postgres" => builder.AddPostgresDatabase(connection, false),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider,
                $"Unsupported database provider {provider}")
        };
    private static DbContextOptionsBuilder AddSqlServerDatabase(
        this DbContextOptionsBuilder builder, 
        string connString,
        bool isDevelopment)
    {
        return (isDevelopment ? builder.EnableSensitiveDataLogging() : builder).UseSqlServer(
            connString,
            optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                optionsBuilder.UseCompatibilityLevel(120);
                optionsBuilder.MigrationsAssembly("OzonCard.Database.Migrations.SqlServer");
            });
    }

    private static DbContextOptionsBuilder AddPostgresDatabase(
        this DbContextOptionsBuilder builder, 
        string connString,
        bool isDevelopment)
    {
        return (isDevelopment ? builder.EnableSensitiveDataLogging() : builder).UseNpgsql(
            connString,
            optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                optionsBuilder.MigrationsAssembly("OzonCard.Database.Migrations.Postgres");
            });
    }
    
}