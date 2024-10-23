using OzonCard.DeferredRequest.Handler;

namespace OzonCard.DeferredRequest.Processor;

internal interface IDeferredRequestProcessors
{
    IEnumerable<IDeferredRequestsHandlerInfo> GetHandlers();
    
    IDeferredRequestsHandler GetHandler(string key);
}