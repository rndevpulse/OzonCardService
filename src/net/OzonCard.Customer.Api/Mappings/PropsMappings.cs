using AutoMapper;
using OzonCard.Common.Application.Properties.Data;
using OzonCard.Common.Domain.Props;
using OzonCard.Customer.Api.Models.Props;

namespace OzonCard.Customer.Api.Mappings;

public class PropsMappings : Profile
{
    public PropsMappings()
    {
        CreateMap<MemberReportBatchProp, ReportBatchModel>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Organization, opt => opt.MapFrom(x => x.Organization))
            .ForMember(x=>x.Properties, opt=> opt.MapFrom(x=>x.GetProperty<IEnumerable<ReportBatchProp>>()))
            ;
    }
}