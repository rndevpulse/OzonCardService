using OzonCard.DeferredRequest.Properties.Extensions;

namespace OzonCard.DeferredRequest.Handler;

public static class DeferredRequestHandlerExtensions
{
    private static bool TryGetProperty<TSettings>(this IEnumerable<ExtensionProperty> properties, string name, 
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
}