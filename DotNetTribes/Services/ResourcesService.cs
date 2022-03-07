using System;
using System.Linq;
using DotNetTribes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly ApplicationContext _applicationContext;

        public ResourcesService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Resources)
                .FirstOrDefault(k => k.KingdomId == kingdomId);
            
            if (kingdom == null)
            {
                return null;
            }

            var kingdomResources = new ResourcesDto();
            foreach (var resource in kingdom.Resources)
            {

                var resourceDto = new ResourceDto
                {
                    Type = resource.Type,
                    Amount = resource.Amount,
                    UpdatedAt = resource.UpdatedAt
                };
                kingdomResources.Resources.Add(resourceDto);
            }

            return kingdomResources;
        }
    }
}