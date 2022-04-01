using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCardService.Attributes;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using Serilog;
using System;
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
        public async Task<ActionResult<object>> UploadCustomersReport(CustomersUpload_vm customers)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.Error(ex, "UploadCustomersReport {@customers}", customers);
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
