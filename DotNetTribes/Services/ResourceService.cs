using System;
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
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
                var building = _applicationContext.Buildings.FirstOrDefault(b =>
                    b.KingdomId == kingdomId && b.Type == GetBuildingTypeByResourceType(resource.Type));
                
                if (building == null) continue;
                
                resource.Generation = _rulesService.BuildingResourceGeneration(building);
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
                    UpdatedAt = r.UpdatedAt,
                    Generation = r.Generation
                })
                .ToList();
            
            var resources = new ResourcesDto
            {
                Resources = kingdomResourceDtoList
            };

            return resources;
        }

        private BuildingType GetBuildingTypeByResourceType(ResourceType resourceType)
        {
            var buildingType = resourceType switch
            {
                ResourceType.Food => BuildingType.Farm,
                ResourceType.Gold => BuildingType.Mine,
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };

            return buildingType;
        }
    }
}