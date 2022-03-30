using AutoMapper;
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
        IOrganizationRepository _repository;
        IMapper _mapper;

        public RepositoryService(IOrganizationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            organization.IsActive = true;

            //test
            var rand = new Random();
            organization.Name = "Ololo #"+rand.Next(0,10);

            //adding
            await _repository.AddOrganization(organization, userId);
            return true;
            
        }

    }
}
