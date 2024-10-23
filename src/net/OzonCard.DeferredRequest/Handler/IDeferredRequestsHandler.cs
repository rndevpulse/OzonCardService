using OzonCard.Common.Core;
using OzonCard.DeferredRequest.Properties.Extensions;

namespace OzonCard.DeferredRequest.Handler;

public interface IDeferredRequestsHandler : IDeferredRequestsHandlerInfo
{
   List<ExtensionProperty> Properties { get; }
    
    Task<IBackgroundTask> AppendAsync(IEnumerable<ExtensionProperty> args, CancellationToken ct = default); 
}