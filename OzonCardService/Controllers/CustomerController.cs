
using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCard.Excel;
using OzonCardService.Attributes;
using OzonCardService.Helpers;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using OzonCardService.Services.TasksManagerProgress.Implementation;
using OzonCardService.Services.TasksManagerProgress.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        IRepositoryService _service;
        ITasksManagerProgress _tasksManager;
        private readonly ILogger log = Log.ForContext(typeof(OrganizationController));

        public CustomerController(IRepositoryService repositoryService, ITasksManagerProgress tasksManager)
        {
            _service = repositoryService;
            _tasksManager = tasksManager;
        }


        [HttpPost("upload")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public async Task<ActionResult<object>> UploadCustomersReport(InfoCustomersUpload_vm infoUpload)
        {
            try
            {
                log.Information("UploadCustomersReport {@infoUpload}", infoUpload);
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );
               
                var customers = new List<ShortCustomerInfo_excel>();
                if (infoUpload.Customer != null)
                    customers.Add(new ShortCustomerInfo_excel()
                    {
                        Card = infoUpload.Customer.Card,
                        Name = infoUpload.Customer.Name,
                    });
                else
                    customers = new ExcelManager(new FileManager().GetFile(infoUpload.FileReport))
                    .GetClients().ToList();
                var progress = new ProgressTask<ProgressInfo>();
                var cancelTokenSource = new CancellationTokenSource();
                var token = cancelTokenSource.Token;
                var t = 
                    _service.UploadCustomers(userId, infoUpload, customers, progress, token);
                
                progress.SetTask(t, cancelTokenSource);
                return _tasksManager.AddTask(progress);
            }
            catch (Exception ex)
            {
                log.Error(ex, "UploadCustomersReport {@customers}", infoUpload);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 400,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }

        
        [HttpPost("search")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<InfoSearchCustomer_dto>>> SearchCustomers(SearchCustomer_vm customer)
        {
            try
            {
                return new OkObjectResult(await _service.SearchCustomers(customer));
            }
            catch (Exception ex)
            {
                log.Error(ex, "SearchCustomers {@customers}", customer);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 400,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }

        [HttpPost("change_category")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public async Task<ActionResult> ChangeCategoryCustomer(ChangeCustomerCategory_vm customer)
        {
            try
            {
                await _service.ChangeCustomerCategory(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex, "ChangeCategoryCustomer {@customers}", customer);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 400,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }


        [HttpPost("change_balance")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public async Task<ActionResult> ChangeBalanceCustomer(ChangeCustomerBalance_vm customer)
        {
            try
            {
                return Ok(await _service.ChangeCustomerBalance(customer));
            }
            catch (Exception ex)
            {
                log.Error(ex, "ChangeBalanceCustomer {@customers}", customer);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 400,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }

    }
}
