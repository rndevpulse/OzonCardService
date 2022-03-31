using AutoMapper;
using OzonCard.BizClient.Services.Interfaces;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzonCardService.Services.Implementation
{
    public class RepositoryService : IRepositoryService
    {
        readonly IOrganizationRepository _repository;
        readonly IMapper _mapper;
        readonly IHttpClientService _client;

        public RepositoryService(IOrganizationRepository repository, IMapper mapper, IHttpClientService httpClientService)
        {
            _repository = repository;
            _mapper = mapper;
            _client = httpClientService;
        }

     

        //взаимодействие с организациями
        public async Task<IEnumerable<Organization_dto>> GetMyOrganizations(Guid userId)
        {
            var orgs = await _repository.GetMyOrganizations(userId);
            return _mapper.Map<IEnumerable<Organization_dto>>(orgs);
        }
        public async Task<bool> AddOrganization(Identity_vm IdentityOrganization, Guid userId)
        {
            //request to iikobiz to get the category and cn (IdentityOrganization.login, IdentityOrganization.pass)
            //if fail req => return false
            var listCat = new List<Category>();
            var cn = new List<CorporateNutrition>();
            var organization = new Organization();
            organization.Categories = listCat;
            organization.CorporateNutritions = cn;
            organization.Login = IdentityOrganization.Login;
            organization.Password = IdentityOrganization.Password;

            var session = await _client.GetSession(IdentityOrganization.Login, IdentityOrganization.Password);
            var response = await _client.GetOrganizations(session);

            //adding
            await _repository.AddOrganization(organization, userId);
            return true;
            
        }

    }
}
