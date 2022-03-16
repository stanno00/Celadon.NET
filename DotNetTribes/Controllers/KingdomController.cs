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
        private readonly IAuthService _authService;

        public KingdomController(IKingdomService kingdomService, IAuthService authService)
        {
            _kingdomService = kingdomService;
            _authService = authService;
        }
        
        // [HttpGet("kingdom")]
        // [Authorize]
        // public ActionResult<KingdomDto> KingdomInfo([FromHeader] string authorization)
        // {
        //
        //     int kingdomId = _authService.GetKingdomIdFromJwt(authorization);
        //     
        //     KingdomDto kingdom = _kingdomService.KingdomInfo(kingdomId);
        //
        //     if (kingdom == null)
        //     {
        //         return new BadRequestObjectResult("Ops something is wrong");
        //     }
        //
        //     return kingdom;
        // }
    }
}