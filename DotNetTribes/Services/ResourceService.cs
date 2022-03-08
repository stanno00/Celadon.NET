using System;
using System.Linq;
using DotNetTribes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationContext _applicationContext;

        public ResourceService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            var kingdom = _applicationContext.Kingdoms.FirstOrDefault(k => k.KingdomId == kingdomId);

            if (kingdom == null)
            {
                return null;
            }
            var resources = _applicationContext.Resources.Where(r => r.Kingdom.KingdomId == kingdomId).AsEnumerable().ToList();

            var kingdomResources = new ResourcesDto();
            foreach (var resourceDto in resources.Select(resource => new ResourceDto
                     {
                         Amount = resource.Amount,
                         Type = resource.Type,
                         UpdatedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
                     }))
            {
                kingdomResources.Resources.Add(resourceDto);
            }

            return kingdomResources;
        }
    }
}