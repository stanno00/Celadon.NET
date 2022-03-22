using System.Linq;
using Microsoft.EntityFrameworkCore;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
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
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .Include(k => k.User)
                .Single(k => k.KingdomId == kingdomId);
            
            KingdomDto kingdomDto = new KingdomDto()
            {
                KingdomName = kingdom.Name,
                Username = kingdom.User?.Username,
                Buildings = kingdom.Buildings,
                Resources = kingdom.Resources,
                Troops = kingdom.Troops
            };
            return kingdomDto;
        }
    }
}