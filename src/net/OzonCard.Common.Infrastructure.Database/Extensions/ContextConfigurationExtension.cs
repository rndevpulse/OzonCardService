using Microsoft.EntityFrameworkCore;

namespace OzonCard.Common.Infrastructure.Database.Extensions;

public static class ContextConfigurationExtension
{
    public static DbContextOptionsBuilder ConfigureContext(this DbContextOptionsBuilder builder, InfrastructureDatabaseOptions options) => 
        builder.ConfigureContext(options.Connection, options.Provider, options.IsDevelopment);

    public static DbContextOptionsBuilder ConfigureContext(
        this DbContextOptionsBuilder builder, 
        string connection,
        string provider,
        bool isDevelopment = false) =>
        provider switch
        {
            "sqlserver" => builder.AddSqlServerDatabase(connection,isDevelopment),
            "postgre" => builder.AddPostgreDatabase(connection, isDevelopment),
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

    private static DbContextOptionsBuilder AddPostgreDatabase(
        this DbContextOptionsBuilder builder, 
        string connString,
        bool isDevelopment)
    {
        return (isDevelopment ? builder.EnableSensitiveDataLogging() : builder).UseNpgsql(
            connString,
            optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                optionsBuilder.MigrationsAssembly("OzonCard.Database.Migrations.Postgre");
            });
    }
    
}