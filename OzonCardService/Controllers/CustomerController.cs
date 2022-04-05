
using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCard.Excel;
using OzonCardService.Attributes;
using OzonCardService.Helpers;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
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
        private readonly ILogger log = Log.ForContext(typeof(OrganizationController));

        public CustomerController(IRepositoryService repositoryService) =>
            _service = repositoryService;



        [HttpPost("upload")]
        [AuthorizeRoles(EnumRules.Basic)]
        [Consumes("application/json")]
        public async Task<ActionResult<InfoDataUpload_dto>> UploadCustomersReport(InfoCustomersUpload_vm infoUpload)
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
                InfoDataUpload_dto info = await _service.UploadCustomers(
                    userId,
                    infoUpload,
                    customers.ToList());

                return info;
            }
            catch (Exception ex)
            {
                log.Error(ex, "UploadCustomersReport {@customers}", infoUpload);
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
