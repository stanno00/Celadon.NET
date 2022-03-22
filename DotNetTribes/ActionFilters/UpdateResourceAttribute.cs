using DotNetTribes.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetTribes.ActionFilters
{
    public class UpdateResourceAttribute : IActionFilter
    {

        private readonly IJwtService _jwtService;
        private readonly IResourceService _resourceService;

        public UpdateResourceAttribute(IJwtService jwtService, IResourceService resourceService)
        {
            _jwtService = jwtService;
            _resourceService = resourceService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("authorization")) return;
            
            var user = (string) context.ActionArguments["authorization"];
            var kingdomId = _jwtService.GetKingdomIdFromJwt(user);
                
            _resourceService.UpdateKingdomResources(kingdomId);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}