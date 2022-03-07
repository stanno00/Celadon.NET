using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController
    {
        private readonly IResourcesService _resourcesService;

        public ResourcesController(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        [HttpGet("{kingdomId}")]
        public ActionResult<ResourcesDto> GetKingdomResources([FromRoute] int kingdomId)
        {
            var kingdomResources = _resourcesService.GetKingdomResources(kingdomId);
            if (kingdomResources == null)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            return new OkObjectResult(kingdomResources);
        }
    }
}