
using OzonCard.Excel;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.TasksManagerProgress.Implementation;
using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzonCardService.Services.Interfaces
{
    public interface IRepositoryService
    {
        //взаимодействие с организациями
        Task<IEnumerable<Organization_dto>> GetMyOrganizations(Guid userId);
        Task<IEnumerable<Organization_dto>> AddOrganizations(Identity_vm IdentityOrganization, Guid userId);
        Task<Organization_dto> UpdateOrganization(Guid organizationId, Guid userId);
        Task<bool> AddUser(Identity_vm identity, string rules);
        Task<IEnumerable<User_dto>> GetUsers();


        Task<bool> AddUserForOrganization(Guid userId, Guid organizationId);
        Task<bool> DelUserForOrganization(Guid userId, Guid organizationId);
        Task SaveFile(Guid id, string format, string name, Guid userId);
        Task RemoveFiles(DateTime dateTime);
        Task UploadCustomers(Guid userId, InfoCustomersUpload_vm infoUpload, List<ShortCustomerInfo_excel> customers, 
            IProgress<ProgressInfo> progress);
        Task<IEnumerable<ReportCN_dto>> CreateReportBiz(Guid userId, ReportOption_vm reportOption);
        Task<IEnumerable<File_dto>> GetFiles(Guid userId);
    }
}
