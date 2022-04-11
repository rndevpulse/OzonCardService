
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
using System.Linq;
using System.Security.Claims;
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
               
                var customers = new ExcelManager(new FileManager().GetFile(infoUpload.FileReport))
                    .GetClients();
                var progress = new ProgressTask<ProgressInfo>();

                var t = 
                    _service.UploadCustomers(
                    userId,
                    infoUpload,
                    customers.ToList(), progress);
                
                progress.SetTask(t);
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
    }
}
