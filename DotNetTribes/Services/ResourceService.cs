using System;
using System.Collections.Generic;
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
            var kingdomResourceDtoList = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .Select(r => new ResourceDto
            {
                Amount = r.Amount,
                Type = r.Type,
                UpdatedAt = DateTimeOffset.Now.ToUnixTimeSeconds()
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