using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetTribes.ActionFilters
{
    public class UpdateResourceAttribute : IActionFilter
    {

        private readonly IJwtService _jwtService;
        private readonly IResourceService _resourceService;
        private readonly ITroopService _troopService;
        private readonly IBattleService _battleService;

        public UpdateResourceAttribute(IJwtService jwtService, IResourceService resourceService, ITroopService troopService, IBattleService battleService)
        {
            _jwtService = jwtService;
            _resourceService = resourceService;
            _troopService = troopService;
            _battleService = battleService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("authorization")) return;
            
            var user = (string) context.ActionArguments["authorization"];
            var kingdomId = _jwtService.GetKingdomIdFromJwt(user);
                
            _resourceService.UpdateKingdomResources(kingdomId);
            _troopService.UpdateTroops(kingdomId);
            _resourceService.UpdateOffers();
            _battleService.UpdateBattles();
            _troopService.ReturnTroopsFromBattle();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}