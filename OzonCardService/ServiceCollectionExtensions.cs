using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.Context;
using OzonCard.Context.Interfaces;
using OzonCard.Context.Repositories;

namespace OzonCardService
{
    static public class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryContextFactory, RepositoryContextFactory>();
            services.AddScoped<ICategoryRepository>(provider => new CategoryRepository(
                configuration.GetConnectionString("DefaultConnection"), 
                provider.GetService<IRepositoryContextFactory>())
            );


            return services;
        }
    }
}
