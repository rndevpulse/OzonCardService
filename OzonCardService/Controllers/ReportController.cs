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
using System.Threading;
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
        public async Task<ActionResult<Guid>> ReportIkoBiz(ReportOption_vm reportOption)
        {
            try
            {
                log.Information("ReportIkoBiz {@reportOption}", reportOption);
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );
                               
                var progress = new ProgressTask<ProgressInfo>();
                progress.Report(new ProgressInfo(new InfoData()));
                var path = new FileManager().GetDirectory();
                var id = Guid.NewGuid();
                var headerexel = $"{reportOption.Title} в период с {DateTime.Parse(reportOption.DateFrom)} по {DateTime.Parse(reportOption.DateTo).AddSeconds(-1)}";
                var cancelTokenSource = new CancellationTokenSource();
                var token = cancelTokenSource.Token;

                var t = Task.Factory.StartNew(async () =>
                {
                    var report = _service.PeriodReportBiz(userId, reportOption, token).Result.ToList();
                    if (token.IsCancellationRequested)
                        return;
                    ExcelManager.CreateWorkbook(
                        Path.Combine(path, id.ToString() + ".xlsx"), 
                        report,
                        headerexel);
                    if (token.IsCancellationRequested)
                        return;
                    await _service.SaveFile(id, "xlsx", reportOption.Title, userId);
                }, token);
                progress.SetTask(t, cancelTokenSource);
                
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



        [HttpPost("transactions")]
        [Consumes("application/json")]
        public async Task<ActionResult<Guid>> TransactionsIkoBiz(ReportOption_vm reportOption)
        {
            try
            {
                log.Information("TransactionsIkoBiz {@reportOption}", reportOption);
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );


                var progress = new ProgressTask<ProgressInfo>();
                progress.Report(new ProgressInfo(new InfoData()));
                var path = new FileManager().GetDirectory();
                var id = Guid.NewGuid();
                var headerexel = $"{reportOption.Title} в период с {DateTime.Parse(reportOption.DateFrom)} по {DateTime.Parse(reportOption.DateTo).AddSeconds(-1)}";
                var cancelTokenSource = new CancellationTokenSource();
                var token = cancelTokenSource.Token;

                var t = Task.Factory.StartNew(async () =>
                {
                    var report = _service.TransactionsReportBiz(userId, reportOption, token).Result;
                    if (token.IsCancellationRequested)
                        return;
                    ExcelManager.CreateWorkbook(
                        Path.Combine(path, id.ToString() + ".xlsx"),
                        report.GetDataSet(),
                        headerexel, false);
                    if (token.IsCancellationRequested)
                        return;
                    await _service.SaveFile(id, "xlsx", reportOption.Title, userId);
                }, token);
                progress.SetTask(t, cancelTokenSource);

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
