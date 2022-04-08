using DotNetTribes.ActionFilters;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Models;
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
        private readonly ITroopService _troopService;
        private readonly IUpgradeService _upgradeService;

        public KingdomController(IKingdomService kingdomService, IJwtService jwtService, IBuildingsService buildingsService, ITroopService troopService, IResourceService resourceService, IUpgradeService upgradeService)
        {
            _resourceService = resourceService;
            _upgradeService = upgradeService;
            _kingdomService = kingdomService;
            _jwtService = jwtService;
            _buildingsService = buildingsService;
            _resourceService = resourceService;
            _troopService = troopService;
            _resourceService = resourceService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<KingdomDto> KingdomInfo([FromHeader] string authorization)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _kingdomService.KingdomInfo(kingdomId);
            return new OkObjectResult(result);
        }
        
        [HttpGet("resource")]
        [Authorize]
        public ActionResult<ResourcesDto> GetKingdomResources([FromHeader] string authorization)
        {
            var kingdomResources = _resourceService.GetKingdomResources(_jwtService.GetKingdomIdFromJwt(authorization));
            if (kingdomResources.Resources.Count == 0)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            return new OkObjectResult(kingdomResources);
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
            var responseBuilding = _buildingsService.UpgradeBuilding(kingdomId, buildingId);
            return new OkObjectResult(responseBuilding);
        }
        
        [Authorize]
        [HttpPost("buildings/upgrades")]
        public ActionResult<BuildingResponseDTO> BuildingAddUpgrade([FromHeader] string authorization,
            [FromBody] BuildingsUpgradesRequestDto upgrade)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var responseBuilding = _upgradeService.AddUpgrade(kingdomId,upgrade);
            return new OkObjectResult(responseBuilding);
        }

        [Authorize]
        [HttpPost("troops")]
        public ActionResult<TroopResponseDTO> CreateTroops([FromHeader] string authorization,
            [FromBody] TroopRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _troopService.TrainTroops(kingdomId, request);
            return new OkObjectResult(result);

        }

        [Authorize]
        [HttpPost("troops/blacksmith")]
        public ActionResult<TroopResponseDTO> CreateSpecialTroop([FromHeader] string authorization,
            [FromBody] TroopRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _troopService.TrainSpecialTroops(kingdomId, request);
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
        public ActionResult<KingdomTroopsDTO> GetKingdomTroops([FromHeader] string authorization)
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
        [HttpPost("offer")]
        public ActionResult<ResponseDTO> Offer([FromHeader] string authorization,
            [FromBody] TradeRequestDTO tradeRequestDto)
        {
            int sellerKingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            Offer accepted = _resourceService.ValidateCreateTradeOffer(sellerKingdomId, tradeRequestDto);

            return new ResponseDTO()
            {
                Status = "ok"
            };
        }

        [Authorize]
        [HttpPut("offer/{offerId}")]
        public ActionResult<ResponseDTO> AcceptingOffer([FromHeader] string authorization, [FromRoute] int offerId)
        {
            int buyerKingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            AcceptOfferResponseDTO accepted = _resourceService.AcceptOffer(buyerKingdomId, offerId);

            return new ResponseDTO
            {
                Status = "ok"
            };
        }

        [Authorize]
        [HttpPut("troops")]
        public ActionResult<KingdomTroopsDTO> UpgradeTroops([FromHeader] string authorization,
            [FromBody] TroopUpgradeRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var result = _troopService.UpgradeTroops(kingdomId, request);

            return new OkObjectResult(result);
        }
    }
}