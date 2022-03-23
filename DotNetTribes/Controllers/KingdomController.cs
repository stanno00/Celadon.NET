using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.ActionFilters;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [Route("kingdom")]
    [ServiceFilter(typeof(UpdateResourceAttribute))]
    public class KingdomController
    {
        private readonly IKingdomService _kingdomService;
        private readonly IJwtService _jwtService;
        private readonly IBuildingsService _buildingsService;
        private readonly ITroopService _troopService;
        
        
        public KingdomController(IKingdomService kingdomService, IJwtService jwtService, IBuildingsService buildingsService, ITroopService troopService)
        {
            _kingdomService = kingdomService;
            _jwtService = jwtService;
            _buildingsService = buildingsService;
            _troopService = troopService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<KingdomDto> KingdomInfo([FromHeader] string authorization)
        {
            return new OkObjectResult(_kingdomService.KingdomInfo(_jwtService.GetKingdomIdFromJwt(authorization)));
        }

        [Authorize]
        [HttpPost("buildings")]
        public ActionResult<KingdomDto> CreateNewBuilding([FromHeader] string authorization, [FromBody] BuildingRequestDTO request)
        {
            return new OkObjectResult(_buildingsService.CreateNewBuilding(_jwtService.GetKingdomIdFromJwt(authorization), request));
        }

        [Authorize]
        [HttpPut("buildings/{buildingId}")]
        public ActionResult<BuildingResponseDTO> UpgradeBuilding([FromHeader] string authorization,
            [FromRoute] int buildingId)
        {
            return new OkObjectResult(_buildingsService.UpgradeBuilding(_jwtService.GetKingdomIdFromJwt(authorization), buildingId));
        }

        [Authorize]
        [HttpPost("troops")]
        public ActionResult<TroopResponseDTO> CreateTroops([FromHeader] string authorization, [FromBody] TroopRequestDTO request)
        {
            return new OkObjectResult(_troopService.TrainNewTroops(_jwtService.GetKingdomIdFromJwt(authorization), request));
        }
        
        [Authorize]
        [HttpGet("troops")]
        public ActionResult<KingdomTroopsDTO> GetKingdomTroops([FromHeader]string authorization)
        {
            var kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            _troopService.UpdateTroops(kingdomId);
            return new OkObjectResult(_troopService.GetKingdomTroops(kingdomId));
        }
        
        [HttpGet("buildings")]
        [Authorize]
        public ActionResult<KingdomBuildingsDto> Buildings([FromHeader] string authorization)
        {
            int id = _jwtService.GetKingdomIdFromJwt(authorization);
            var buildings = _kingdomService.GetExistingBuildings(id);
            return new KingdomBuildingsDto()
            {
                Buildings = buildings
            };
        }
    }
}