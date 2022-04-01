using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCard.Data.Models;
using OzonCardService.Attributes;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OzonCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IRepositoryService _service;
        private readonly ILogger log = Log.ForContext(typeof(OrganizationController));

        public UserController(IRepositoryService repositoryService) =>
            _service = repositoryService;


        [HttpPost("create")]
        [AuthorizeRoles(EnumRules.Admin)]
        [Consumes("application/json")]
        public async Task<ActionResult<bool>> CreateUser(Identity_vm identity)
        {
            
            log.Verbose("CreateUser {@identity}", identity);
            try
            {
                var rules = string.Join(',', new[] { (int)EnumRules.Basic, (int)EnumRules.Basic });
                return new OkObjectResult(await _service.AddUser(identity, rules));
            }
            catch (Exception ex)
            {
                log.Error(ex, "CreateOrganization {@identity}", identity);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 401,
                    Message = ex.Message,
                    Description = ex.Message + " " + ex.InnerException?.ToString()
                });
            }
        }

        [HttpGet("list")]
        [AuthorizeRoles(EnumRules.Admin)]
        public async Task<ActionResult<IEnumerable<User_dto>>> GetUsers()
        {

            log.Verbose("GetUsers");
            try
            {
                return new OkObjectResult(await _service.GetUsers());
            }
            catch (Exception ex)
            {
                log.Error(ex, "GetUsers");
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }


        [HttpGet("{userId}/add_organization/{organizationId}")]
        [AuthorizeRoles(EnumRules.Admin)]
        public async Task<ActionResult<bool>> UserAddOrganization(Guid userId, Guid organizationId)
        {
            log.Verbose("UserAddOrganization org = {@organizationId}, user = {@userId}", organizationId, userId);
            try
            {
                return new OkObjectResult(await _service.AddUserForOrganization(userId, organizationId));
            }
            catch (Exception ex)
            {
                log.Error(ex, "UserAddOrganization org =  {@organizationId}, user = {@userId}", organizationId, userId);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = ex.Message,
                    Description = ex.InnerException?.ToString()
                });
            }
        }


        [HttpGet("{userId}/del_organization/{organizationId}")]
        [AuthorizeRoles(EnumRules.Admin)]
        public async Task<ActionResult<bool>> UserDelOrganization(Guid userId, Guid organizationId)
        {
            log.Verbose("UserDelOrganization org = {@organizationId}, user = {@userId}", organizationId, userId);
            try
            {
                return new OkObjectResult(await _service.DelUserForOrganization(userId, organizationId));
            }
            catch (Exception ex)
            {
                log.Error(ex, "UserDelOrganization org =  {@organizationId}, user = {@userId}", organizationId, userId);
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
