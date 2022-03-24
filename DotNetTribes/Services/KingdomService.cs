using System;
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
                Buildings = kingdom.Buildings.Select(b => new BuildingResponseDTO
                {
                    Hp = b.Hp,
                    Level = b.Level,
                    Finished_at = b.Finished_at,
                    Started_at = b.Started_at,
                    Type = b.Type.ToString()
                })
                    .ToList(),
                Resources = kingdom.Resources.Select(r => new ResourceDto
                {
                    Amount = r.Amount,
                    Type = r.Type.ToString(),
                    UpdatedAt = r.UpdatedAt,
                    Generation = r.Generation
                })
                    .ToList(),
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

            var resultX = Math.Abs(myKingdomX - kingdomUnderAttackX);
            var resultY = Math.Abs(myKingdomY - kingdomUnderAttackY);
            
            if (resultX >= resultY)
            {
                return resultX * 2;
            }
            
            return resultY * 2;
        }

        public List<BuildingResponseDTO> GetExistingBuildings(int kingdomId)
        {
            var buildings = _applicationContext.Buildings
                .Where(k => k.KingdomId == kingdomId)
                .Select(b => new BuildingResponseDTO()
                {
                    Id = b.BuildingId,
                    Type = b.Type.ToString(),
                    Level = b.Level,
                    Hp = b.Hp,
                    Started_at = b.Started_at,
                    Finished_at = b.Finished_at
                }).ToList();
            return buildings;
        }
    }
}