using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.DeferredRequest.Processor;

internal class DeferredRequestProcessors(
    IServiceProvider serviceProvider,
    Assembly[] assemblies
) : IDeferredRequestProcessors
{
    private record RequestProcessor(Type Type, IDeferredRequestsHandlerInfo HandlerInfo);
    static IDictionary<string, RequestProcessor>? _handlers;
    private IDictionary<string, RequestProcessor> Handlers => _handlers ??= InitRequestHandlers();
    
  

    private IDictionary<string, RequestProcessor> InitRequestHandlers()
    {
        var handlerTypes = assemblies.GetTypesAssignableFrom<IDeferredRequestsHandler>();
        var result = new Dictionary<string, RequestProcessor>();
        foreach (var handler in handlerTypes)
        {
            string? key = null;
            try
            {
                var service = (IDeferredRequestsHandler)serviceProvider.GetRequiredService(handler);
                key = service.Key;
                result.Add(service.Key, new RequestProcessor(handler, service));
            }
            catch (Exception e)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DeferredRequestProcessors>>();
                logger.LogError(e, "Fail registration service '{handlerType}' with key '{key}'",
                    handler.FullName ?? handler.Name,
                    key ?? "unknown");
            }
        }
        return result;
    }

    public IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers()
    {
        return Handlers
            .Select(x=>x.Value.HandlerInfo)
            .OrderBy(x=>x.Key)
            .ToArray();
    }

    public IDeferredRequestsHandler GetHandler(string key) =>
        (IDeferredRequestsHandler)serviceProvider
            .GetRequiredService(Handlers[key].Type);

}