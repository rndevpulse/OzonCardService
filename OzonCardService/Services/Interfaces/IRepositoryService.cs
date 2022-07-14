
using OzonCard.Excel;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.TasksManagerProgress.Implementation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonCardService.Services.Interfaces
{
    public interface IRepositoryService
    {
        //взаимодействие с организациями
        Task<IEnumerable<Organization_dto>> GetMyOrganizations(Guid userId);
        Task<IEnumerable<Organization_dto>> AddOrganizations(Identity_vm IdentityOrganization, Guid userId);
        Task<Organization_dto> UpdateOrganization(Guid organizationId, Guid userId);
        Task<bool> AddUser(UserCreate_vm user);
        Task<IEnumerable<User_dto>> GetUsers();


        Task<bool> AddUserForOrganization(Guid userId, Guid organizationId);
        Task<bool> DelUserForOrganization(Guid userId, Guid organizationId);
        Task UploadCustomers(Guid userId, InfoCustomersUpload_vm infoUpload, List<ShortCustomerInfo_excel> customers, 
            IProgress<ProgressInfo> progress, CancellationToken token);
        Task<IEnumerable<ReportCN_dto>> PeriodReportBiz(Guid userId, ReportOption_vm reportOption, CancellationToken token);
        Task<TransactionsReport> TransactionsReportBiz(Guid userId, ReportOption_vm reportOption, CancellationToken token);
        Task SaveFile(Guid id, string format, string name, Guid userId);
        Task RemoveFile(string url);
        Task<IEnumerable<File_dto>> GetFiles(Guid userId);


        Task<IEnumerable<InfoSearchCustomer_dto>> SearchCustomers(SearchCustomer_vm customer);
    }
}
