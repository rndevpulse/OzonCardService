
using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;

namespace OzonCard.DeferredRequest.Manager;

public interface IDeferredRequestsManager
{

    IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers();
    IEnumerable<ExtensionProperty> GetPropertiesHandler(string key);

    Task<IBackgroundTask> AppendRequestAsync(string key,
        DateTimeOffset schedule,
        int timeOffset,
        IEnumerable<ExtensionProperty> args,
        IDictionary<string, object>? features = null,
        CancellationToken ct = default);

}