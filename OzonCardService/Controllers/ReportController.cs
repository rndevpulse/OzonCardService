using Microsoft.AspNetCore.Http;
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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        IRepositoryService _service;
        ITasksManagerProgress _tasksManager;
        private readonly ILogger log = Log.ForContext(typeof(ReportController));

        public ReportController(IRepositoryService repositoryService, ITasksManagerProgress tasksManager)
        {
            _service = repositoryService;
            _tasksManager = tasksManager;
        }




        [HttpPost]
        [AuthorizeRoles(EnumRules.Report)]
        [Consumes("application/json")]
        public async Task<ActionResult<object>> UploadCustomersReport(ReportOption_vm reportOption)
        {
            try
            {
                log.Information("UploadCustomersReport {@reportOption}", reportOption);
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );

                
                var progress = new ProgressTask<ProgressInfo<IInfoData>>();
                var path = new FileManager().GetDirectory();
                var id = Guid.NewGuid();
                var title = $"{reportOption.Title} from {reportOption.DateFrom} to {reportOption.DateTo}";
                var t = new Task(async () =>
                {
                    ExcelManager.CreateWorkbook(
                        Path.Combine(path, id.ToString() + ".xlsx"),
                        _service.CreateReportBiz(userId, reportOption).Result.ToList(),
                        title);
                    await _service.SaveFile(id, ".xlsx", title);
                });
                progress.SetTask(t);
                return _tasksManager.AddTask(progress);
            }
            catch (Exception ex)
            {
                log.Error(ex, "UploadCustomersReport {@reportOption}", reportOption);
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
