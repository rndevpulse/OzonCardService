using AutoMapper;
using OzonCard.Data.Models;
using OzonCardService.Models.DTO;

namespace OzonCardService.Helpers
{
    public class MappingProfileContext : Profile
    {
		public MappingProfileContext()
		{
			CreateMap<Organization, Organization_dto>();
			CreateMap<Category, Category_dto>();
			CreateMap<CorporateNutrition, CorporateNutrition_dto>();
			CreateMap<User, User_dto>();

		}
	}
}
