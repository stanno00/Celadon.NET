using System.Collections.Generic;
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
                Buildings = kingdom.Buildings,                    
                Resources = kingdom.Resources,
                Troops = kingdom.Troops
            };
            return kingdomDto;
        }
        
        public List<BuildingResponseDTO> GetExistingBuildings(int kingdomId)
        {
            var buildings = _applicationContext.Buildings
                .Where(k => k.KingdomId == kingdomId)
                .Select(b => new BuildingResponseDTO()
                {
                    Id = b.BuildingId,
                    Type = b.Type.ToString(),
                    Level = b.Level,
                    Hp = b.Hp,
                    Started_at = b.Started_at,
                    Finished_at = b.Finished_at
                }).ToList();
            return buildings;
        }
    }
}