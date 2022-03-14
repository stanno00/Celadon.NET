using System;
using System.Linq;
using DotNetTribes.DTOs;
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

        private void UpdateResources(int kingdomId)
        {
            // This method updates amount of resources everytime it is called
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();

            foreach (var resource in kingdomResources)
            {
                resource.Amount = _timeService.TimeSince(resource.CreatedAt) * resource.Generation;
            }
            
            _applicationContext.SaveChanges();
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            // Updating resources
            UpdateResources(kingdomId);
            
            var kingdomResourceDtoList = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .Select(r => new ResourceDto
            {
                Amount = r.Amount,
                Type = r.Type.ToString(),
                UpdatedAt = (int) DateTimeOffset.Now.ToUnixTimeSeconds() / 60
            })
                .AsEnumerable()
                .ToList();
            
            var resources = new ResourcesDto
            {
                Resources = kingdomResourceDtoList
            };
            
            return resources;
        }
    }
}