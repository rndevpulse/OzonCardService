using OzonCard.Data.Models;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzonCardService.Services.Interfaces
{
    public interface IRepositoryService
    {
        //взаимодействие с организациями
        Task<IEnumerable<Organization_dto>> GetMyOrganizations(Guid userId);
        Task<bool> AddOrganization(Identity_vm IdentityOrganization, Guid userId);

    }
}
