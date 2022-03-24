using DotNetTribes.ActionFilters;
 using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Troops;
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
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _kingdomService.KingdomInfo(kingdomId);
            return new OkObjectResult(result);

        }

        [Authorize]
        [HttpPost("buildings")]
        public ActionResult<KingdomDto> CreateNewBuilding([FromHeader] string authorization,
            [FromBody] BuildingRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _buildingsService.CreateNewBuilding(kingdomId, request);
            return new OkObjectResult(result);
        }

        [Authorize]
        [HttpPut("buildings/{buildingId}")]
        public ActionResult<BuildingResponseDTO> UpgradeBuilding([FromHeader] string authorization,
            [FromRoute] int buildingId)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _buildingsService.UpgradeBuilding(kingdomId, buildingId);
            return new OkObjectResult(result);
        }

        [Authorize]
        [HttpPost("troops")]
        public ActionResult<TroopResponseDTO> CreateTroops([FromHeader] string authorization, [FromBody] TroopRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _troopService.TrainNewTroops(kingdomId, request);
            return new OkObjectResult(result);
        }

        [Authorize]
        [HttpGet("nearest/{minutes}")]
        public ActionResult<NearbyKingdomsDto> NearestKingdoms([FromRoute] int minutes,
            [FromHeader] string authorization)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var response = _kingdomService.NearestKingdoms(minutes, kingdomId);

            return new ObjectResult(response);
        }

        [Authorize]
        [HttpGet("troops")]
        public ActionResult<KingdomTroopsDTO> GetKingdomTroops([FromHeader]string authorization)
        {
            var kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            _troopService.UpdateTroops(kingdomId);
            return new OkObjectResult(_troopService.GetKingdomTroops(kingdomId));
        }
        
        [Authorize]
        [HttpGet("buildings")]
        public ActionResult<KingdomBuildingsDto> Buildings([FromHeader] string authorization)
        {
            int id = _jwtService.GetKingdomIdFromJwt(authorization);
            var buildings = _kingdomService.GetExistingBuildings(id);
            return new KingdomBuildingsDto()
            {
                Buildings = buildings
            };
        }
        
        [Authorize]
        [HttpPut("troops")]
        public ActionResult<KingdomTroopsDTO> UpgradeTroops([FromHeader]string authorization, [FromBody]TroopUpgradeRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _troopService.UpgradeTroops(kingdomId, request);

            return new OkObjectResult(result);
        }
    }
}