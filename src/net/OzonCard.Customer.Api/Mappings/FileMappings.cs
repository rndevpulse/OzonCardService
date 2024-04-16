using AutoMapper;
using OzonCard.Common.Domain.Files;
using OzonCard.Customer.Api.Models.Files;

namespace OzonCard.Customer.Api.Mappings;

public class FileMappings : Profile
{
    protected FileMappings()
    {
        CreateMap<SaveFile, FileModel>()
            .ForCtorParam(nameof(FileModel.Id), e => e.MapFrom(x => x.Id))
            .ForCtorParam(nameof(FileModel.Url), e => e.MapFrom(x => $"{x.Id}.{x.Format}"))
            .ForCtorParam(nameof(FileModel.Name), e => e.MapFrom(x => x.Name))
            ;
    }
}