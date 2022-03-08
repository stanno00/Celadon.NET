using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Service
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext ApplicationContext;

        public KingdomService(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public KingdomDto KingdomInfo(UserDto userDto)
        {
            Kingdom kingdom = null;
            
            try
            {
                kingdom = ApplicationContext.Kingdoms
                    .Include(k => k.Buildings)
                    .Include(k => k.Resources)
                    .Include(k => k.Troops)
                    // this needs to be changed depends how i will need to find token
                    .Single(k => k.KingdomId == userDto.UserId);
            }
            catch (Exception e)
            {
                return null;
            }

            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                UserName = "Hrnik",
                Buildings = kingdom.Buildings,
                Resources = kingdom.Resources,
                Troops = kingdom.Troops
            };
            
            return kingdomDto;
            
        }
    }
}