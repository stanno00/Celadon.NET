using System;
using DotNetTribes.DTOs;
using DotNetTribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    public class KingdomController
    {
        private readonly IKingdomService _kingdomService;
        private readonly IJwtService _jwtService;

        public KingdomController(IKingdomService kingdomService, IJwtService jwtService)
        {
            _kingdomService = kingdomService;
            _jwtService = jwtService;
        }
        
        [HttpGet("kingdom")]
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
    }
}