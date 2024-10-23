using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OzonCard.DeferredRequest.Attributes;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.DeferredRequest.Processor;

internal class DeferredRequestProcessors(
    IServiceProvider serviceProvider,
    ILogger<DeferredRequestProcessors> logger
) : IDeferredRequestProcessors
{
    private record RequestProcessor(Type Type, IDeferredRequestsHandlerInfo HandlerInfo);
    static IDictionary<string, RequestProcessor>? _handlers;
    private IDictionary<string, RequestProcessor> Handlers => _handlers ??= InitRequestHandlers();
    
  

    private IDictionary<string, RequestProcessor> InitRequestHandlers()
    {
        var handlerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x=>x.GetCustomAttributes(typeof(DeferredProcessorAttribute), true).Any());
        var result = new Dictionary<string, RequestProcessor>();
        foreach (var handler in handlerTypes)
        {
            var attribute = handler.GetCustomAttribute<DeferredProcessorAttribute>();
            if (attribute == null) 
                continue;
            try
            {
                var service = (IDeferredRequestsHandler)serviceProvider.GetRequiredService(handler);
                result.Add(service.Key, new RequestProcessor(handler, service));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
        return result;
    }

    public IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers()
    {
        return Handlers
            .Select(x=>x.Value.HandlerInfo)
            .ToArray();
    }

    public IDeferredRequestsHandler GetHandler(string key) =>
        (IDeferredRequestsHandler)serviceProvider
            .GetRequiredService(Handlers[key].Type);

}