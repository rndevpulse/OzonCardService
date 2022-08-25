using AutoMapper;
using OzonCard.Data.Models;
using OzonCardService.Models.DTO;
using System.Collections.Generic;
using System.Linq;

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
			CreateMap<FileReport, File_dto>()
				.ForMember(m => m.Url, opt => opt.MapFrom(b => b.Id + "." + b.Format));

			CreateMap<Customer, InfoSearchCustomer_dto>()
				.ForMember(m => m.Card, opt => opt.MapFrom(b => b.Cards.First(x => x.IsActive).Number))
				.ForMember(m => m.Organization, opt => opt.MapFrom(b => b.Organization.Name))
				.ForMember(m => m.TabNumber, opt => opt.MapFrom(b => b.TabNumber))
				.ForMember(m => m.Balance, opt => opt.MapFrom(b => b.Wallets.First().Balance))
				.ForMember(m => m.Categories, opt => opt.MapFrom(b => b.Categories.Select(x => x.Category.Name)))
				.ForMember(m=>m.TimeUpdateBalance, opt=>opt.MapFrom(b=> b.Wallets.First().Update))
				;
			CreateMap<CustomerReport, ReportCN_dto>();
		}
	}
}
