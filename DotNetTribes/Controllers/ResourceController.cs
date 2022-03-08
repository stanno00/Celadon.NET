using System.Collections.Generic;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController
    {
        private readonly IResourceService _resourcesService;

        public ResourceController(IResourceService resourceService)
        {
            _resourcesService = resourceService;
        }

        [HttpGet("{kingdomId}")]
        public ActionResult<List<ResourceDto>> GetKingdomResources([FromRoute] int kingdomId)
        {
            var kingdomResources = _resourcesService.GetKingdomResources(kingdomId);
            if (kingdomResources.Resources.Count == 0)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            return new OkObjectResult(kingdomResources);
        }
    }
}