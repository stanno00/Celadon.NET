﻿using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    [Route("kingdom")]
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
            
            KingdomDto kingdom = _kingdomService.KingdomInfo(kingdomId);

            if (kingdom == null)
            {
                return new BadRequestObjectResult("Ops something is wrong");
            }

            return new OkObjectResult(kingdom);
        }

        [Authorize]
        [HttpPost("buildings")]
        public ActionResult<KingdomDto> CreateNewBuilding([FromHeader] string authorization, [FromBody] BuildingRequestDTO request)
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
        [HttpPost("troops")]
        public ActionResult<TroopResponseDTO> CreateTroops([FromHeader] string authorization, [FromBody] TroopRequestDTO request)
        {
            int kingdomId = _jwtService.GetKingdomIdFromJwt(authorization);
            var response = _troopService.CreateNewTroops(kingdomId, request);

            return new OkObjectResult(response);
        }
    }
}