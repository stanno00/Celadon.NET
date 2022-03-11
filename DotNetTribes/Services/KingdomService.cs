using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Service
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _applicationContext;

        public KingdomService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public KingdomDto KingdomInfo(int kingdomId)
        {
            Kingdom kingdom = null;
            
            try
            {
                kingdom = _applicationContext.Kingdoms
                    .Include(k => k.Buildings)
                    .Include(k => k.Resources)
                    .Include(k => k.Troops)
                    .Single(k => k.KingdomId == kingdomId);
            }
            catch (Exception e)
            {
                return null;
            }

            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                UserName = kingdom.User.Username,
                Buildings = kingdom.Buildings,
                Resources = kingdom.Resources,
                Troops = kingdom.Troops
            };
            
            return kingdomDto;
            
        }
    }
}