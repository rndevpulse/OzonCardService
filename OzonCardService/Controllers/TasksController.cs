using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCardService.Attributes;
using OzonCardService.Models.DTO;
using OzonCardService.Services.TasksManagerProgress.Interfaces;
using Serilog;
using System;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {

        ITasksManagerProgress _tasksManager;
        private readonly ILogger log = Log.ForContext(typeof(TasksController));

        public TasksController(ITasksManagerProgress tasksManager) =>
            _tasksManager = tasksManager;

        [HttpGet("{taskId}")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public ActionResult<IInfoData> GetTaskInformation(Guid taskId)
        {
            try
            {
                log.Information("GetTaskInformation {@taskId}", taskId);
                var obj = _tasksManager.GetStatusTask(taskId);
                return new ActionResult<IInfoData>(obj);
            }
            catch (Exception ex)
            {
                log.Error(ex, "GetTaskInformation {@taskId}", taskId);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }
        [HttpGet("cancel/{taskId}")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public ActionResult CancelTask(Guid taskId)
        {
            try
            {
                log.Information("CancelTask {@taskId}", taskId);
                _tasksManager.CancelTask(taskId);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex, "GetTaskInformation {@taskId}", taskId);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }

    }
}
