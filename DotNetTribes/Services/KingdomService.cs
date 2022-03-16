using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            IdIsNullOrDoesNotExist(kingdomId);

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

        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request)
        {
            IdIsNullOrDoesNotExist(kingdomId);
            
            var kingdom = _applicationContext.Kingdoms
                .Include(b => b.Buildings)
                .Include(r => r.Resources)
                .FirstOrDefault(k => k.KingdomId == kingdomId);

            var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type.Equals(ResourceType.Gold));
            
            //TODO: remove this after Rules get implemented
            int buildingPrice = 100;

            if (request.Type == null)
            {
                throw new BuildingCreationException("Building type required.");
            }

            if (!Enum.TryParse(request.Type, true, out BuildingType requestedBuilding))
            {
                throw new BuildingCreationException("Incorrect building type.");
            }

            if (buildingPrice > kingdomGold.Amount)
            {
                throw new BuildingCreationException("Gold needed.");
            }

            Building toBeAdded = new Building
            {
                //TODO: these are placeholder values, update after Rules get implemented
                Type = requestedBuilding,
                Level = 1,
                Hp = 100,
                Started_at = 1000000,
                Finished_at = 1500000,
                KingdomId = kingdomId
            };

            kingdomGold.Amount -= buildingPrice;
            kingdom.Buildings.Add(toBeAdded);
            _applicationContext.SaveChanges();

            return new BuildingResponseDTO
            {
                Type = toBeAdded.Type.ToString(),
                Level = toBeAdded.Level,
                Hp = toBeAdded.Hp,
                Started_at = toBeAdded.Started_at.ToString(),
                Finished_at = toBeAdded.Finished_at.ToString(),
                Id = toBeAdded.BuildingId
            };
        }

        private void IdIsNullOrDoesNotExist(int kingdomId)
        {

            if (kingdomId == 0 || !_applicationContext.Kingdoms.Any(k => k.KingdomId == kingdomId))
            {
                throw new KingdomDoesNotExistException();
            }
        }
    }
}