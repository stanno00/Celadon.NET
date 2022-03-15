using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;
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

        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request)
        {
            var kingdom = _applicationContext.Kingdoms
                    .Include(b => b.Buildings)
                    .FirstOrDefault(k => k.KingdomId == kingdomId);
           
            var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type == "gold");
            
            //dummy values
            int buildingPrice = 500;

            if (request.Type == null)
            {
                throw new BuildingCreationException("Building type required.");
            }
            
            if (buildingPrice > kingdomGold.Amount)
            {
                throw new BuildingCreationException("Gold needed.");
            }
             
            //placeholder
            if (request.Type == "incorrect")
            {
                throw new BuildingCreationException("Incorrect building type.");
            }

            Building toBeAdded = new Building
            {
                //placeholder values
                Type = request.Type,
                Level = 1,
                Hp = 100,
                Started_at = 1000000,
                Finished_at = 1500000,
                KingdomId = kingdomId
            };
            
            kingdomGold.Amount -= buildingPrice;
            kingdom.Buildings.Add(toBeAdded);
            
            return new BuildingResponseDTO
            {
                Type = toBeAdded.Type,
                Level = toBeAdded.Level,
                Hp = toBeAdded.Hp,
                Started_at = toBeAdded.Started_at.ToString(),
                Finished_at = toBeAdded.Finished_at.ToString(),
                Id = toBeAdded.BuildingId
            };
        }
        
        private void IdIsNullOrDoesNotExist(int kingdomId)
        {
            if (kingdomId == 0 || _applicationContext.Kingdoms
                    .Last().KingdomId > kingdomId)
            {
               throw new KingdomDoesNotExistException();
            }
        }
    }
}