using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetTribes.ActionFilters
{
    public class UpdateResourceAttribute : IActionFilter
    {

        private readonly IJwtService _jwtService;
        private readonly IResourceService _resourceService;
        private readonly ITroopService _troopService;
        private readonly IUpgradeService _upgradeService;

        public UpdateResourceAttribute(IJwtService jwtService, IResourceService resourceService, ITroopService troopService, IUpgradeService upgradeService)
        {
            _jwtService = jwtService;
            _resourceService = resourceService;
            _troopService = troopService;
            _upgradeService = upgradeService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("authorization")) return;
            
            var user = (string) context.ActionArguments["authorization"];
            var kingdomId = _jwtService.GetKingdomIdFromJwt(user);
                
            _resourceService.UpdateKingdomResources(kingdomId);
            _troopService.UpdateTroops(kingdomId);
            _resourceService.UpdateOffers();
            _upgradeService.ApplyUpgradesWhenFinished(kingdomId);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}