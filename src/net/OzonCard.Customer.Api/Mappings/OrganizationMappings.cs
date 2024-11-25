using AutoMapper;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Customer.Api.Models.Organizations;

namespace OzonCard.Customer.Api.Mappings;

public class OrganizationMappings : Profile
{
    public OrganizationMappings()
    {
        CreateMap<Organization, OrganizationModel>();
        CreateMap<Category, CategoryModel>();
        CreateMap<Common.Domain.Organizations.Program, ProgramModel>();
    }
}