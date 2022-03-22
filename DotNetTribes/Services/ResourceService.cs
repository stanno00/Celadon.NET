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
        private readonly IRulesService _rules;

        public ResourceService(ApplicationContext applicationContext, ITimeService timeService, IRulesService rules)
        {
            _applicationContext = applicationContext;
            _timeService = timeService;
            _rules = rules;
        }

        private void UpdateSingleResource(Resource resource)
        {
            var minutesPassedSinceLastUpdate = (int)_timeService.MinutesSince(resource.UpdatedAt);

            // Unless a minute passes this method wont run
            if (minutesPassedSinceLastUpdate <= 0) return;
            
            resource.Amount += minutesPassedSinceLastUpdate * resource.Generation;
            resource.UpdatedAt = _timeService.GetCurrentSeconds();

        }

        private void UpdateKingdomResources(int kingdomId)
        {
            // Getting all resources from kingdom 
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
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
        
        public void FeedTroops(int kingdomId)
        {
            //TODO: Discuss whether the consuming flag is actually needed - unfinished troops not eating is already addressed by its model.
            var kingdomFood = _applicationContext.Resources.FirstOrDefault(r => r.Type == ResourceType.Food && r.KingdomId == kingdomId);
            var kingdomTroops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId)
                .ToList();
            int consumption = 0;
            //First, adjust the generation.
            foreach (var troop in kingdomTroops)
            {
                consumption += _rules.TroopFoodConsumption(troop.Level);
            }
            // The method can't just lower food generation every time it's run - it has to calculate the whole amount, so the implementation of generation is needed here.
            
            //kingdomFood.Generation = for farm in farms{generation += rules.FarmFoodGeneration(Farm.Level) - consumption}
            
            //then feed the troops
            
            /*
             * if (_timeService.TimeSince(troop.FedAt) <= 60)
             * {
             *if (enough food)
             * {kingdomFood -= consumption}
             * }
             * else
             * {fertilizer hits the ventilator here}             * 
             */
        }
    }
}