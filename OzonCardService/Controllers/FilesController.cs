using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCardService.Attributes;
using OzonCardService.Helpers;
using OzonCardService.Models.DTO;
using OzonCardService.Services.Interfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        IRepositoryService _service;
        private readonly ILogger log = Log.ForContext(typeof(OrganizationController));

        public FileController(IRepositoryService repositoryService) =>
            _service = repositoryService;


        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        [AuthorizeRoles(EnumRules.Basic)]
        public async Task<ActionResult<object>> DocCreate(IFormFile file)
        {
            try
            {
                var id = Guid.NewGuid();
                log.Verbose("POST /files/create: {0}", file.FileName);
                if (!await new FileManager().Save(id, file))
                    throw new Exception("File format not correct");
                
                var format = file.FileName.Split(".").Last().Trim().ToLower();
                await _service.SaveFile(id, format);
                return new OkObjectResult(new
                {
                    id = id,
                    url = id + "." + format
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }




        [HttpGet("get/{name}")]
        public ActionResult<IFormFile> GetFile(string name)
        {
            try
            {
                var fm = new FileManager();
                return PhysicalFile(
                    fm.GetFile(name),
                    "multipart/form-data");
            }
            catch (Exception ex)
            {
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
