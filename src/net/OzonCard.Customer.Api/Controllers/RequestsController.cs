using Microsoft.AspNetCore.Mvc;
using OzonCard.Customer.Api.Models.BackgroundTask;
using OzonCard.Customer.Api.Models.Requests;
using OzonCard.DeferredRequest.Configuration.Properties.Extensions;
using OzonCard.DeferredRequest.Handler;
using OzonCard.DeferredRequest.Manager;

namespace OzonCard.Customer.Api.Controllers;

public class RequestsController(
    IDeferredRequestsManager manager
) : ApiController
{

    [HttpGet]
    public IEnumerable<IDeferredRequestsHandlerInfo> Index() =>
        manager.GetHandlers();

    [HttpGet("{key}/[action]")]
    public IEnumerable<ExtensionProperty> Properties(string key) =>
        manager.GetPropertiesHandler(key);

    [HttpPost]
    public async Task<BackgroundTaskModel> AppendRequest(string key, 
        RequestModel model, CancellationToken ct = default)
    {
        var features = new Dictionary<string, object>
        {
            { "userId", UserClaimSid },
            { "user", UserClaimEmail ?? "Unknown"},
        };
        var task = await manager.AppendRequestAsync(
            key, 
            model.Schedule, 
            model.TimeOffset,
            model.Properties,
            features,
            ct
        );
        return Mapper.Map<BackgroundTaskModel>(task);
    }
    
    
}