using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OzonCard.Context;
using OzonCard.Context.Interfaces;
using OzonCard.Context.Repositories;
using OzonCardService.Helpers;
using OzonCardService.Services.Implementation;
using OzonCardService.Services.Interfaces;

namespace OzonCardService
{
    static public class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryContextFactory, RepositoryContextFactory>();

            services.AddScoped<IOrganizationRepository>(provider => new OrganizationRepository(
                configuration.GetConnectionString("DefaultConnection"), 
                provider.GetService<IRepositoryContextFactory>())
            );
            services.AddScoped<IIdentityRepository>(provider => new IdentityRepository(
                configuration.GetConnectionString("DefaultConnection"),
                provider.GetService<IRepositoryContextFactory>())
            );
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IIdentityService, IdentityService>();


            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

            return services;
        }
    }
}
