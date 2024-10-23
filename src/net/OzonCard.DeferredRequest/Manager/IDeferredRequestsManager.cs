
using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Properties.Extensions;

namespace OzonCard.DeferredRequest.Manager;

public interface IDeferredRequestsManager
{

    IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers();
    IEnumerable<ExtensionProperty> GetPropertiesHandler(string key);

    Task<IBackgroundTask> AppendRequestAsync(string key, IEnumerable<ExtensionProperty> args,
        CancellationToken ct = default);
    
}