using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ITimeService _timeService;
        private readonly IRulesService _rulesService;

        public ResourceService(ApplicationContext applicationContext, ITimeService timeService, IRulesService rulesService)
        {
            _applicationContext = applicationContext;
            _timeService = timeService;
            _rulesService = rulesService;
        }

        private void UpdateSingleResource(Resource resource)
        {
            var minutesPassedSinceLastUpdate = (int)_timeService.MinutesSince(resource.UpdatedAt);

            // Unless a minute passes this method wont run
            if (minutesPassedSinceLastUpdate <= 0) return;

            resource.Amount += minutesPassedSinceLastUpdate * resource.Generation;
            resource.UpdatedAt = _timeService.GetCurrentSeconds();

        }

        public void UpdateKingdomResources(int kingdomId)
        {
            // Getting all resource generation buildings from kingdom 
            var kingdomResourceBuildings = _applicationContext.Buildings
                .Where(b => b.KingdomId == kingdomId && (b.Type == BuildingType.Farm || b.Type == BuildingType.Mine))
                .ToList();

            // Updating each resource
            foreach (var building in kingdomResourceBuildings)
            {
                var resource = _applicationContext.Resources.FirstOrDefault(r =>
                    r.KingdomId == kingdomId && r.Type == GetResourceTypeByBuildingType(building.Type));
                
                if (resource == null) continue;
                
                resource.Generation = _rulesService.BuildingResourceGeneration(building, resource);
                UpdateSingleResource(resource);
            }
            _applicationContext.SaveChanges();
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            // Updating all resources before returning values to controller
            UpdateKingdomResources(kingdomId);
            
            var kingdomResourceDtoList = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .Select(r => new ResourceDto
                {
                    Amount = r.Amount,
                    Type = r.Type.ToString(),
                    UpdatedAt = r.UpdatedAt
                })
                .ToList();
            
            var resources = new ResourcesDto
            {
                Resources = kingdomResourceDtoList
            };

            return resources;
        }

        private ResourceType GetResourceTypeByBuildingType(BuildingType buildingType)
        {
            var resourceType = buildingType switch
            {
                BuildingType.Farm => ResourceType.Food,
                BuildingType.Mine => ResourceType.Gold,
                _ => throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null)
            };

            return resourceType;
        }
    }
}