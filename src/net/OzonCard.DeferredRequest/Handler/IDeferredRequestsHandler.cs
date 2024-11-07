using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Configuration;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;

namespace OzonCard.DeferredRequest.Handler;

public interface IDeferredRequestsHandler : IDeferredRequestsHandlerInfo
{
   IEnumerable<ExtensionProperty> Properties { get; }
    
    Task<IBackgroundTask> AppendAsync(
        RequestConfiguration configuration,
        CancellationToken ct = default); 
}