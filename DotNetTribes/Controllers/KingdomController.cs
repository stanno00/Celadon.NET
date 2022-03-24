using DotNetTribes.ActionFilters;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.Enums;
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
        private readonly IResourceService _resourceService;
        
        public KingdomController(IKingdomService kingdomService, IJwtService jwtService, IBuildingsService buildingsService, IResourceService resourceService)
        {
            _kingdomService = kingdomService;
            _jwtService = jwtService;
            _buildingsService = buildingsService;
            _resourceService = resourceService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<KingdomDto> KingdomInfo([FromHeader] string authorization)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);

            KingdomDto kingdom = _kingdomService.KingdomInfo(kingdomId);

            if (kingdom == null)
            {
                return new BadRequestObjectResult("Ops something is wrong");
            }

            return new OkObjectResult(kingdom);
        }

        [Authorize]
        [HttpPost("buildings")]
        public ActionResult<KingdomDto> CreateNewBuilding([FromHeader] string authorization,
            [FromBody] BuildingRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var response = _buildingsService.CreateNewBuilding(kingdomId, request);

            return new OkObjectResult(response);
        }

        [Authorize]
        [HttpPut("buildings/{buildingId}")]
        public ActionResult<BuildingResponseDTO> UpgradeBuilding([FromHeader] string authorization,
            [FromRoute] int buildingId)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var response = _buildingsService.UpgradeBuilding(kingdomId, buildingId);
            return new OkObjectResult(response);
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

        [Authorize]
        [HttpPost("offer")]
        public ActionResult<ResponseDTO> Offer([FromHeader] string authorization,
            [FromBody] TradeRequestDTO tradeRequestDto)
        {
            int id = _jwtService.GetKingdomIdFromJwt(authorization);
            bool accepted = _resourceService.ValidateTradeOffer(id, tradeRequestDto);
            return new ResponseDTO()
            {
                status = "ok"
            };
        }
    }
}