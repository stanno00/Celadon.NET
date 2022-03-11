using System;
using DotNetTribes.DTOs;
using DotNetTribes.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    public class KingdomController
    {
        private readonly IKingdomService _kingdomService;
        private readonly IAuthService _authService;

        public KingdomController(IKingdomService kingdomService, IAuthService authService)
        {
            _kingdomService = kingdomService;
            _authService = authService;
        }
        
        [HttpGet("kingdom")]
        [Authorize]
        public ActionResult<KingdomDto> KingdomInfo([FromBody] LoginResponseDto token)
        {

            string kingdomId = _authService.GetKingdomIdFromJwt(token.Token);
            
            KingdomDto kingdom = _kingdomService.KingdomInfo(Convert.ToInt32(kingdomId));

            if (kingdom == null)
            {
                return new BadRequestObjectResult("Ops something is wrong");
            }

            return kingdom;
        }
    }
}