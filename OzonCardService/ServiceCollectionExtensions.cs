using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OzonCard.BizClient.HttpClients;
using OzonCard.BizClient.Services.Implementation;
using OzonCard.BizClient.Services.Interfaces;
using OzonCard.Context;
using OzonCard.Context.Interfaces;
using OzonCard.Context.Repositories;
using OzonCardService.Helpers;
using OzonCardService.Services.Implementation;
using OzonCardService.Services.Interfaces;
using OzonCardService.Services.Quartzs.Jobs;
using OzonCardService.Services.Quartzs.Workers;
using OzonCardService.Services.TasksManagerProgress.Implementation;
using OzonCardService.Services.TasksManagerProgress.Interfaces;
using Quartz;
using Quartz.Plugin.Interrupt;
using System;
using System.Globalization;

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
            services.AddScoped<IServiceRepository>(provider => new ServiceRepository(
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



        public static IServiceCollection AddBizClient(this IServiceCollection services)
        {
            services.AddHttpClient<IClient, Client>(c =>
            {
                c.BaseAddress = new Uri(HttpClientService.URL);
                //c.Timeout = TimeSpan.FromSeconds(10);
            });
            services.AddScoped<IHttpClientService, HttpClientService>();

            return services;
        }

        public static IServiceCollection AddManagerTasksProgress(this IServiceCollection services)
        {
            services.AddScoped<ITasksManagerProgress, TasksManagerProgress>();
            return services;
        }


        
        public static IServiceCollection AddQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddTransient<IServiceDatabase, ServiceDatabase>();
            //services.AddTransient<Workers.IDeleteOld, Workers.DeleteOld>();
            //services.AddTransient<Workers.IModerationsAuto, Workers.ModerationsAuto>();
            services.AddQuartz(q =>
            {
                // base quartz scheduler, job and trigger configuration
                q.UseMicrosoftDependencyInjectionJobFactory();
                //выставление независимого времени
                q.UseTimeZoneConverter();

                q.ScheduleJob<BackupDatabaseJob>(t => t
                    .WithIdentity("Backup_Job", "Backup")
                    .UsingJobData(
                        JobInterruptMonitorPlugin.JobDataMapKeyMaxRunTime,
                        TimeSpan.FromHours(1).TotalMilliseconds.ToString(CultureInfo.InvariantCulture))
                    .WithSchedule(CronScheduleBuilder
                        .DailyAtHourAndMinute(1, 0)
                        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")))
                    );


            });
            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            return services;
        }
    }
}
