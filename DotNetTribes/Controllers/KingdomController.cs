using DotNetTribes.DTOs;
using DotNetTribes.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTribes.Controllers
{
    public class KingdomController
    {
        private readonly IKingdomService KingdomService;

        public KingdomController(IKingdomService kingdomService)
        {
            KingdomService = kingdomService;
        }
        
        [HttpGet("kingdom")]
        public ActionResult<KingdomDto> KingdomInfo([FromBody] UserDto userDto)
        {

            KingdomDto kingdom = KingdomService.KingdomInfo(userDto);

            if (kingdom == null)
            {
                return new BadRequestObjectResult("Ops something is wrong");
            }

            return kingdom;
        }
    }
}