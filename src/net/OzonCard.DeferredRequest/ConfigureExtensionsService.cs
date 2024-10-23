using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Manager;
using OzonCard.DeferredRequest.Processor;

namespace OzonCard.DeferredRequest;

public static class ConfigureExtensionsService
{
    public static IServiceCollection AddDeferredRequests(this IServiceCollection services)
    {
        services.AddScoped<IDeferredRequestsManager, DeferredRequestsManager>();
        services.AddScoped<IDeferredRequestProcessors, DeferredRequestProcessors>();
        services.AddAssignableScoped<IDeferredRequestsHandler>([Assembly.GetExecutingAssembly()]);

        return services;
    }
    
    public static IServiceCollection AddAssignableScoped<T>(this IServiceCollection services,
        IEnumerable<Assembly> assemblies) =>
        AddAssignableScoped<T>(services, assemblies.ToArray());
    
    
    public static IServiceCollection AddAssignableScoped<T>(this IServiceCollection services, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            assembly.GetTypesAssignableFrom(typeof(T)).ForEach(type=>
                services.AddScoped(type)
            );
        return services;
    }
    
    static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType) =>
        assembly.DefinedTypes
            .Where(type => compareType.IsAssignableFrom(type) && compareType != type)
            .Cast<Type>().ToList();

}