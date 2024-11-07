using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;

namespace OzonCard.DeferredRequest.Handler;

public static class DeferredRequestHandlerExtensions
{
    public static bool TryGetProperty<TSettings>(this IEnumerable<ExtensionProperty> properties, string name, 
        out TSettings value) where TSettings : ExtensionProperty, new()
    {
        value = new TSettings();
        var setting = properties.FirstOrDefault(x =>
            x is TSettings && x.Name == name);
        if (setting == null)
            return false;
        value = (TSettings)setting;
        return true;
    }  
    public static bool TryGetProperty<TSettings>(this IDeferredRequestsHandler handler, string name, 
        out TSettings value) where TSettings : ExtensionProperty, new()
        => handler.Properties.TryGetProperty(name, out value);
    
    
    
    
    
    public static bool TryGetProperty<TSettings>(this RequestConfiguration configuration, string name, 
        out TSettings value) where TSettings : ExtensionProperty, new()
        => configuration.Properties.TryGetProperty(name, out value);
    
    public static TSettings GetProperty<TSettings>(this RequestConfiguration configuration, string name) where TSettings : ExtensionProperty, new()
        => configuration.Properties.TryGetProperty(name, out TSettings value)
        ? value
        : throw new Exception($"Property {name} not found");

    public static T? TryGetFeature<T>(this RequestConfiguration configuration, string featureName)
    {
        if (configuration.Features?.TryGetValue(featureName, out var feature) == true)
            return (T)feature;
        return default;
    }
}