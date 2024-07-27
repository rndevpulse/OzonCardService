using AutoMapper;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Data;
using OzonCard.Customer.Api.Models.BackgroundTask;

namespace OzonCard.Customer.Api.Mappings;

public class BackgroundTaskMappings : Profile
{
    
    public  BackgroundTaskMappings()
    {
        CreateMap<IBackgroundTask, BackgroundTaskModel>();
    }
}