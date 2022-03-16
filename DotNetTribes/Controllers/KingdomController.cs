using System;
using DotNetTribes.DTOs;
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

        public KingdomController(IKingdomService kingdomService, IJwtService jwtService)
        {
            _kingdomService = kingdomService;
            _jwtService = jwtService;
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

            return kingdom;
        }

        // [Authorize]
        [HttpPost("buildings")]
        public ActionResult<KingdomDto> CreateNewBuilding([FromHeader] string authorization, [FromBody] BuildingRequestDTO request)
        {
            // int kingdomId = _authService.GetKingdomIdFromJwt(authorization);
            int kingdomId = 1;
            var response = _kingdomService.CreateNewBuilding(kingdomId, request);

            return new OkObjectResult(response);
        }
    }
}