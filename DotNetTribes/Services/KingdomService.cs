using System.Linq;
using Microsoft.EntityFrameworkCore;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IResourceService _resourceService;

        public KingdomService(ApplicationContext applicationContext, IResourceService resourceService)
        {
            _applicationContext = applicationContext;
            _resourceService = resourceService;
        }

        public KingdomDto KingdomInfo(int kingdomId)
        {
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .Include(k => k.User)
                .Single(k => k.KingdomId == kingdomId);
            
            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                Username = kingdom.User?.Username,
                Buildings = kingdom.Buildings,
                Resources = _resourceService.GetKingdomResources(kingdomId),
                Troops = kingdom.Troops
            };
            return kingdomDto;
        }
    }
}