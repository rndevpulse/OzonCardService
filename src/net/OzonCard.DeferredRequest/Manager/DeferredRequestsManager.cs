using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Processor;
using OzonCard.DeferredRequest.Properties.Extensions;

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
        return processors.GetHandler(key).Properties.AsReadOnly();
    }

    public Task<IBackgroundTask> AppendRequestAsync(string key, IEnumerable<ExtensionProperty> args,
        CancellationToken ct = default)

    {
        return processors.GetHandler(key).AppendAsync(args, ct);
    }
}