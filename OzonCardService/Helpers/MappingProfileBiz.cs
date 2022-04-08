using AutoMapper;
using BizModel = OzonCard.BizClient.Models;
using RepModel = OzonCard.Data.Models;

namespace OzonCardService.Helpers
{
    public class MappingProfileBiz : Profile
    {
		public MappingProfileBiz()
		{
			CreateMap<BizModel.Organization, RepModel.Organization>();
			CreateMap<BizModel.Category, RepModel.Category>();
			CreateMap<BizModel.CorporateNutrition, RepModel.CorporateNutrition>()
				.ForMember(m => m.isActive, opt => opt.MapFrom(b => b.serviceTo == null || b.serviceTo > System.DateTime.UtcNow ? true : false))
				.ForMember(m => m.Description, opt => opt.MapFrom(b => b.description == null ? string.Empty : b.description));
			CreateMap<BizModel.Wallet, RepModel.Wallet>();
			CreateMap<BizModel.ReportCN, Models.DTO.ReportCN_dto>();

		}
	}
}
