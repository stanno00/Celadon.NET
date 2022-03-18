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

        public ResourceService(ApplicationContext applicationContext, ITimeService timeService)
        {
            _applicationContext = applicationContext;
            _timeService = timeService;
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
    }
}