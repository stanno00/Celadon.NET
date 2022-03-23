using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;

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

        public object NearestKingdoms(int minutes, int kingdomId)
        {
            if (minutes < 2)
            {
                throw new KingdomReceiveMinutesLessThanTwo("It takes at least two minutes to move one square!");
            }

            List<NearbyKingdomsDto> kingdomsDtos = new List<NearbyKingdomsDto>();
            var myKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == kingdomId);

            foreach (var kingdom in _applicationContext.Kingdoms)
            {
                int minutesToKingdom = ShortestPath(myKingdom.KingdomX, myKingdom.KingdomY, kingdom.KingdomX,
                    kingdom.KingdomY);
                if (minutesToKingdom <= minutes && myKingdom.KingdomId != kingdom.KingdomId)
                {
                    kingdomsDtos.Add(new NearbyKingdomsDto()
                    {
                        KingdomId = kingdom.KingdomId,
                        KingdomName = kingdom.Name,
                        KingdomCoordinateX = kingdom.KingdomX,
                        KingdomCoordinateY = kingdom.KingdomY,
                        MinutesToArrive = minutesToKingdom
                    });
                }
            }

            return kingdomsDtos;
        }

        public int ShortestPath(int myKingdomX, int myKingdomY, int kingdomUnderAttackX, int kingdomUnderAttackY)
        {
            int minutes = 0;

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is top left
                if (myKingdomX > kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX--;
                        myKingdomY--;
                        minutes++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is top right
                if (myKingdomX < kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX++;
                        myKingdomY--;
                        minutes++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is bottom left
                if (myKingdomX > kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX--;
                        myKingdomY++;
                        minutes++;
                    }
                }
            }

            if (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
            {
                // enemy is bottom right
                if (myKingdomX < kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
                {
                    while (myKingdomX != kingdomUnderAttackX && myKingdomY != kingdomUnderAttackY)
                    {
                        myKingdomX++;
                        myKingdomY++;
                        minutes++;
                    }
                }
            }

            // kingdoms are on the same X 
            if (myKingdomX == kingdomUnderAttackX && myKingdomY > kingdomUnderAttackY)
            {
                while (myKingdomY != kingdomUnderAttackY)
                {
                    myKingdomY--;
                    minutes++;
                }
            }

            if (myKingdomX == kingdomUnderAttackX && myKingdomY < kingdomUnderAttackY)
            {
                while (myKingdomY != kingdomUnderAttackY)
                {
                    myKingdomY++;
                    minutes++;
                }
            }

            // kingdoms are on the same Y
            if (myKingdomY == kingdomUnderAttackY && myKingdomX > kingdomUnderAttackX)
            {
                while (myKingdomX != kingdomUnderAttackX)
                {
                    myKingdomX--;
                    minutes++;
                }
            }

            if (myKingdomY == kingdomUnderAttackY && myKingdomX < kingdomUnderAttackX)
            {
                while (myKingdomX != kingdomUnderAttackX)
                {
                    myKingdomX++;
                    minutes++;
                }
            }

            return minutes * 2;
        }
    }
}