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

            var kingdomBuildingDtoList = _applicationContext.Buildings
                .Where(b => b.KingdomId == kingdomId)
                .Select(b => new BuildingDTO
                {
                    Type = b.Type.ToString(),
                    StartedAt = b.Started_at,
                    FinishedAt = b.Finished_at,
                    BuildingId = b.BuildingId,
                    HP = b.Hp,
                    KingdomId = b.KingdomId,
                    Level = b.Level
                })
                .ToList();

            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                Username = kingdom.User?.Username,
                Buildings = new BuildingsDTO
                {
                    Buildings = kingdomBuildingDtoList
                },
                Resources = _resourceService.GetKingdomResources(kingdomId),
                Troops = kingdom.Troops
            };
            return kingdomDto;
        }
    }
}