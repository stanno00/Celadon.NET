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
        public ActionResult<ResourcesDto> GetKingdomResources([FromRoute] int kingdomId)
        {
            var kingdomResources = _resourcesService.GetKingdomResources(kingdomId);
            if (kingdomResources == null)
            {
                return new NotFoundObjectResult("Kingdom not found!");
            }

            return new OkObjectResult(kingdomResources);
        }
    }
}