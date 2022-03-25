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

        private void UpdateSingleResource(Resource resource, int kingdomId)
        {
            var minutesPassedSinceLastUpdate = (int) _timeService.MinutesSince(resource.UpdatedAt);

            // Unless a minute passes this method won't run
            if (minutesPassedSinceLastUpdate <= 0) return;


            //In the case that we have an army larger than we can feed, we try to do so before adding the new food to the kingdom's stores,
            //so that consumption is adjusted for how much food we get in this tick. This intends to prevent a scenario where we reach negative food.
            if (resource.Type == ResourceType.Food && resource.Generation < 0)
            {
                FeedTroopsWithInsufficientFood(kingdomId);
            }

            resource.Amount += minutesPassedSinceLastUpdate * resource.Generation;
            resource.UpdatedAt = _timeService.GetCurrentSeconds();
        }

        public void UpdateKingdomResources(int kingdomId)
        {
            // Getting all resource generating buildings from the kingdom 
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
                var building = _applicationContext.Buildings.FirstOrDefault(b =>
                    b.KingdomId == kingdomId && b.Type == GetBuildingTypeByResourceType(resource.Type));

                if (building == null) continue;

                //resource.Generation = _rulesService.BuildingResourceGeneration(building);
                resource.Generation = resource.Type == ResourceType.Food
                    ? (_rulesService.BuildingResourceGeneration(building) - CalculateTroopConsumption(kingdomId))
                    : _rulesService.BuildingResourceGeneration(building);
                UpdateSingleResource(resource, kingdomId);
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

        private int CalculateTroopConsumption(int kingdomId)
        {
            var kingdomTroops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId && t.ConsumingFood)
                .ToList();
            int consumption = 0;
            foreach (var troop in kingdomTroops)
            {
                if (troop.ConsumingFood)
                {
                    consumption += _rulesService.TroopFoodConsumption(troop.Level);
                }
            }

            return consumption;
        }

        private void FeedTroopsWithInsufficientFood(int kingdomId)
        {
            var kingdomFood = _applicationContext.Resources
                .Single(r => r.Type == ResourceType.Food && r.KingdomId == kingdomId);
            var kingdomTroops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId && t.ConsumingFood)
                .OrderByDescending(t => t.Level)
                .ToList();
            foreach (var troop in kingdomTroops)
            {
                int foodNeeded = _rulesService.TroopFoodConsumption(troop.Level);
                if (kingdomFood.Amount > foodNeeded)
                {
                    kingdomFood.Amount -= foodNeeded;
                    troop.UpdatedAt = _timeService.GetCurrentSeconds();
                }
                else
                {
                    _applicationContext.Troops.Remove(troop);
                }
            }

            //The moment troops starve to death, they stop consuming food. Therefore, the value needs to be recalculated:
            kingdomFood.Generation = CalculateTroopConsumption(kingdomId);
            _applicationContext.SaveChanges();
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