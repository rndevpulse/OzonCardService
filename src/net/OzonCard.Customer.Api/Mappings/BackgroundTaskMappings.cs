using AutoMapper;
using OzonCard.Common.Worker.Data;
using OzonCard.Customer.Api.Models.BackgroundTask;

namespace OzonCard.Customer.Api.Mappings;

public class BackgroundTaskMappings : Profile
{
    
    public  BackgroundTaskMappings()
    {
        CreateMap<BackgroundTask,BackgroundTaskModel>()
            .ForCtorParam(nameof(BackgroundTaskModel.Id), e=>e.MapFrom(x=>x.Id))
            .ForCtorParam(nameof(BackgroundTaskModel.QueuedAt), e=>e.MapFrom(x=>x.QueuedAt))
            .ForCtorParam(nameof(BackgroundTaskModel.CompletedAt), e=>e.MapFrom(x=>x.CompletedAt))
            .ForCtorParam(nameof(BackgroundTaskModel.Status), e=>e.MapFrom(x=>x.Status))
            .ForCtorParam(nameof(BackgroundTaskModel.Error), e=>e.MapFrom(x=>x.Error))
            .ForCtorParam(nameof(BackgroundTaskModel.Progress), e=>e.MapFrom(x=>x.Progress))

        ;
    }
}