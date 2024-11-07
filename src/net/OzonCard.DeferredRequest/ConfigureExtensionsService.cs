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
    
    
    public static IServiceCollection AddAssignableScoped<T>(this IServiceCollection services, 
        Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            assembly.GetTypesAssignableFrom<T>().ForEach(type=>
                services.AddScoped(type)
            );
        return services;
    }
    
    public static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
    {
        var assignableType = typeof(T);
        return assembly.DefinedTypes
            .Where(type => assignableType.IsAssignableFrom(type) && assignableType != type)
            .Cast<Type>().ToList();
    }

}