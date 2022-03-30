using Microsoft.AspNetCore.Mvc;
using OzonCard.Data.Enums;
using OzonCard.Data.Models;
using OzonCardService.Attributes;
using OzonCardService.Models.DTO;
using OzonCardService.Models.View;
using OzonCardService.Services.Interfaces;
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
        public OrganizationController(IRepositoryService repositoryService) =>
            _service = repositoryService;

        [Route("list")]
        [HttpGet]
        [AuthorizeRoles(EnumRules.Basic)]
        public async Task<IEnumerable<Organization_dto>> GetMyOrganizations()
        {
            Guid userId = new Guid();
            await Task.Run(() => Guid.TryParse(
                    User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
            );
            return await _service.GetMyOrganizations(userId);
        }


        [Route("create")]
        [HttpPost]
        [AuthorizeRoles(EnumRules.Admin)]
        [Consumes("application/json")]
        public async Task<object> CreateOrganization(Identity_vm identity)
        {
            Guid userId = new Guid();
            await Task.Run(() => Guid.TryParse(
                    User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out userId)
            );
            return await _service.AddOrganization(identity, userId);
        }
    }
}
