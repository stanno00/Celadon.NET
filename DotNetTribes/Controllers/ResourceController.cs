using System.Collections.Generic;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController
    {
        private readonly IResourceService _resourcesService;
        private readonly IJwtService _jwtService;

        public ResourceController(IResourceService resourceService, IJwtService jwtService)
        {
            _resourcesService = resourceService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<ResourceDto>> GetKingdomResources([FromHeader] string jwtToken)
        {
            var kingdomResources = _resourcesService.GetKingdomResources(_jwtService.GetKingdomIdFromJwt(jwtToken));
            if (kingdomResources.Resources.Count == 0)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            return new OkObjectResult(kingdomResources);
        }
    }
}