using AutoMapper;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Customer.Api.Models.Organizations;

namespace OzonCard.Customer.Api.Mappings;

public class OrganizationMappings : Profile
{
    public OrganizationMappings()
    {
        CreateMap<Organization, OrganizationModel>()
            .ForMember(x=>x.Categories, opt=>opt.MapFrom(x=>x.Categories.Where(c=>c.IsActive)))
            .ForMember(x=>x.Programs, opt=>opt.MapFrom(x=>x.Programs));
        CreateMap<Category, CategoryModel>();
        CreateMap<Common.Domain.Organizations.Program, ProgramModel>();
    }
}