﻿using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCardService.Attributes;
using OzonCardService.Helpers;
using OzonCardService.Models.DTO;
using Serilog;
using System;
using System.Threading.Tasks;

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
        public ActionResult<InfoDataUpload_dto> GetTaskInformation(Guid taskId)
        {
            try
            {
                log.Information("GetTaskInformation {@taskId}", taskId);
                return _tasksManager.GetStatusTask(taskId);
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
