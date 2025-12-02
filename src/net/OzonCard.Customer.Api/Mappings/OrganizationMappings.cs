using AutoMapper;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Customer.Api.Models.Organizations;

namespace OzonCard.Customer.Api.Mappings;

public class OrganizationMappings : Profile
{
    public OrganizationMappings()
    {
        CreateMap<Organization, OrganizationModel>();
        CreateMap<Category, CategoryModel>()
            .ConstructUsing(x=> new CategoryModel(x.CategoryId, x.Name, x.IsActive));
        CreateMap<Common.Domain.Organizations.Program, ProgramModel>()
            .ConstructUsing(x=>new ProgramModel(x.ProgramId,x.Name));
    }
}