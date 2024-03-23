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
    public class OrganizationController : Controller
    {
        IRepositoryService _service;
        private readonly ILogger log = Log.ForContext(typeof(OrganizationController));

        public OrganizationController(IRepositoryService repositoryService) =>
            _service = repositoryService;

        [HttpGet("list")]
        [AuthorizeRoles(EnumRules.Basic)]
        public async Task<IEnumerable<Organization_dto>> GetMyOrganizations()
        {
            Guid.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value, out var userId);
            log.Verbose("GetMyOrganizations {@userId}", userId);
            return await _service.GetMyOrganizations(userId);
        }


        [HttpGet("{organizationId}/update")]
        [AuthorizeRoles(EnumRules.Basic)]
        public async Task<ActionResult<Organization_dto>> UpdateOrganization(Guid organizationId)
        {
            Guid.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value, out var userId);
            log.Verbose("UpdateOrganization = {@organizationId}, user = {@userId}", organizationId, userId);
            try
            {
                return new OkObjectResult(await _service.UpdateOrganization(userId, organizationId));
            }
            catch (Exception ex)
            {
                log.Error(ex, "UpdateOrganization = {@organizationId}, user = {@userId}", organizationId, userId);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = "Wrong login or password",
                    Description = ex.Message + " " + ex.InnerException?.ToString()
                });
            }
        }

        [HttpPost("create")]
        [AuthorizeRoles(EnumRules.Admin)]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Organization_dto>>> CreateOrganization(Identity_vm identity)
        {
            Guid.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value, out var userId);
            log.Verbose("CreateOrganization {@identity}", identity);
            try
            {
                return new OkObjectResult(await _service.AddOrganizations(identity, userId));
            }
            catch (Exception ex)
            {
                log.Error(ex, "CreateOrganization {@identity}", identity);
                return new BadRequestObjectResult(new Error_dto
                {
                    Code = 404,
                    Message = "Wrong login or password",
                    Description = ex.Message + " " + ex.InnerException?.ToString()
                });
            }
        }
    }
}
