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
using System.Security.Claims;
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
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );
                var id = Guid.NewGuid();
                log.Verbose("POST /files/create: {0} user {1}", file.FileName);
                if (!await new FileManager().Save(id, file))
                    throw new Exception("File format not correct");
                var str = file.FileName.Split(".");
                var format = str.Last().Trim().ToLower();
                await _service.SaveFile(id, format, file.FileName, userId);
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

        [HttpGet("user")]
        [AuthorizeRoles(EnumRules.Basic)]
        public async Task<ActionResult<File_dto>> DocsUser()
        {
            try
            {
                Guid userId = new Guid();
                await Task.Run(() => Guid.TryParse(
                        User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
                );

                log.Verbose("POST /files/user: {0} ", userId);
                var files = await _service.GetFiles(userId);
                return new OkObjectResult(files.OrderByDescending(x=>x.Created));
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
