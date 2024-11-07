using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Processor;

namespace OzonCard.DeferredRequest.Manager;

internal class DeferredRequestsManager(
    IDeferredRequestProcessors processors
) : IDeferredRequestsManager
{
    public IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers()
    {
        return processors.GetHandlers();
    }

    public IEnumerable<ExtensionProperty> GetPropertiesHandler(string key)
    {
        return processors.GetHandler(key).Properties;
    }

    public Task<IBackgroundTask> AppendRequestAsync(string key, 
        DateTimeOffset schedule,
        int timeOffset,
        IEnumerable<ExtensionProperty> args,
        IDictionary<string, object>? features = null,
        CancellationToken ct = default)

    {
        return processors.GetHandler(key).AppendAsync(
            new RequestConfiguration(
                schedule,
                timeOffset,
                args,
                features),
            ct
            );
    }
}