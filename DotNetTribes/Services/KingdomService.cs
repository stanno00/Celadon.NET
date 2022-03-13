using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;
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

            IdIsNullOrDoesNotExist(kingdomId);
            
            var kingdom = _applicationContext.Kingdoms
                    .Include(k => k.Buildings)
                    .Include(k => k.Resources)
                    .Include(k => k.Troops)
                    .Single(k => k.KingdomId == kingdomId);

            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                Username = kingdom.User.Username,
                Buildings = kingdom.Buildings,
                Resources = kingdom.Resources,
                Troops = kingdom.Troops
            };
            
            return kingdomDto;
        }
        
        private void IdIsNullOrDoesNotExist(int kingdomId)
        {
            if (kingdomId == 0 || _applicationContext.Kingdoms.Last().KingdomId > kingdomId)
            {
               throw new KingdomDoesNotExistException();
            }
        }
    }
}