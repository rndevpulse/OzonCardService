using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Manager;
using OzonCard.DeferredRequest.Processor;

namespace OzonCard.DeferredRequest;

public static class ConfigureExtensionsService
{
    public static IServiceCollection AddDeferredRequests(this IServiceCollection services, Assembly[]? assembly = null)
    {
        assembly ??= [Assembly.GetCallingAssembly()];
        services.AddScoped<IDeferredRequestsManager, DeferredRequestsManager>();
        services.AddScoped<IDeferredRequestProcessors, DeferredRequestProcessors>(x=> 
            new DeferredRequestProcessors(x, assembly));
        services.AddAssignableScoped<IDeferredRequestsHandler>(assembly);

        return services;
    }
    
    internal static IServiceCollection AddAssignableScoped<T>(this IServiceCollection services,
        IEnumerable<Assembly> assemblies) =>
        AddAssignableScoped<T>(services, assemblies.ToArray());
    
    
    internal static IServiceCollection AddAssignableScoped<T>(this IServiceCollection services, 
        Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            assembly.GetTypesAssignableFrom<T>().ForEach(type=>
                services.AddScoped(type)
            );
        return services;
    }

    internal static List<Type> GetTypesAssignableFrom<T>(this Assembly[] assemblies) =>
        assemblies.SelectMany(assembly => assembly.GetTypesAssignableFrom<T>())
            .ToList();
    internal static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
    {
        var assignableType = typeof(T);
        return assembly.DefinedTypes
            .Where(type => assignableType.IsAssignableFrom(type) && assignableType != type)
            .Cast<Type>().ToList();
    }

}