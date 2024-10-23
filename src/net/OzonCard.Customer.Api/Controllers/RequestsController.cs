using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Manager;
using OzonCard.DeferredRequest.Properties.Extensions;

namespace OzonCard.Customer.Api.Controllers;

public class RequestsController(
    IDeferredRequestsManager manager
) : ApiController
{

    [HttpGet]
    public IEnumerable<IDeferredRequestsHandlerInfo> Index() =>
        manager.GetHandlers();

    [HttpGet("{key:string}/[action]")]
    public IEnumerable<ExtensionProperty> Properties(string key) =>
        manager.GetPropertiesHandler(key);

    [HttpPost]
    public async Task<BackgroundTaskModel> AppendRequest(string key, IEnumerable<ExtensionProperty> properties, CancellationToken ct = default)
    {
        var task = await manager.AppendRequestAsync(key, properties, ct);
        return Mapper.Map<BackgroundTaskModel>(task);
    }
    
    
}