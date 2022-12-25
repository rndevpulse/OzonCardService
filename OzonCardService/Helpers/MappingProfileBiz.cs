using AutoMapper;
using BizModel = OzonCard.BizClient.Models;
using RepModel = OzonCard.Data.Models;
using System.Linq;
using System;

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
			CreateMap<BizModel.ReportCN, RepModel.CustomerReport>();


			CreateMap<BizModel.Customer, Models.DTO.InfoSearchCustomer_dto>()
				.ForMember(m=>m.Card, opt=> opt.MapFrom(b=>b.cards.First(x=>x.isActivated).number))
				.ForMember(m => m.Organization, opt => opt.MapFrom(b => b.comment))
				.ForMember(m => m.TabNumber, opt => opt.MapFrom(b => b.userData))
				.ForMember(m => m.Balance, opt => opt.MapFrom(b => b.walletBalances.First().balance))
				.ForMember(m => m.Categories, opt => opt.MapFrom(b => b.categories
					.Where(x=>x.isActive).Select(x=>x.name)))
				.ForMember(m=>m.TimeUpdateBalance, opt=>opt.MapFrom(b=>DateTime.UtcNow))
				;

			CreateMap<BizModel.TransactionsReport, Models.DTO.TransactionsReport_dto>()
				.ForMember(m=>m.Date, opt=>opt.MapFrom(b=>b.transactionCreateDate.ToString("yyyy-MM-dd")))
				.ForMember(m=>m.Time, opt=>opt.MapFrom(b=>b.transactionCreateDate.ToString("HH:mm:ss")))
				.ForMember(m=>m.СardNumbers, opt=>opt.MapFrom(b=>b.cardNumbers))
				;

			CreateMap<BizModel.TransactionsReport, RepModel.Event>()
				.ForMember(m => m.Create, opt => opt.MapFrom(b => b.transactionCreateDate))
				.ForMember(m => m.CardNumbers, opt => opt.MapFrom(b => b.cardNumbers))
				;
		}
	}
}
