using AutoMapper;
using OzonCard.Data.Models;
using OzonCardService.Models.DTO;

namespace OzonCardService.Helpers
{
    public class MappingProfile : Profile
    {
		public MappingProfile()
		{
			CreateMap<Organization, Organization_dto>();
			
		}
	}
}
