using System;
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
            /* actual value
            var kingdom = _applicationContext.Kingdoms
                    .Include(b => b.Buildings)
                    .FirstOrDefault(k => k.KingdomId == kingdomId);
                    
                    var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold); */
            
            //dummy kingdom
            var user = new User
            {
                UserId = 1,
                Username = "Yanari",
                Email = "j@k.cz",
            };

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Name = "Yanariland",
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                

            };

            kingdom.User = user;
            user.Kingdom = kingdom;
            kingdom.Resources.Add(new Resource
            {
                Amount = 500,
                Kingdom = kingdom,
                KingdomId = 1,
                ResourceId = 1,
                Type = ResourceType.Gold
            });

            var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type.Equals(ResourceType.Gold));
            int buildingPrice = 1000;
            BuildingType btypes;


            if (request.Type == null)
            {
                throw new BuildingCreationException("Building type required.");
            }
            
            if (!Enum.TryParse(request.Type, true, out btypes))
            {
                throw new BuildingCreationException("Incorrect building type.");
            }
            
            if (buildingPrice > kingdomGold.Amount)
            {
                throw new BuildingCreationException("Gold needed.");
            }

            var requested = btypes;


            Building toBeAdded = new Building
                {
                    //placeholder values
                    Type = requested,
                    Level = 1,
                    Hp = 100,
                    Started_at = 1000000,
                    Finished_at = 1500000,
                    KingdomId = kingdomId,
                    BuildingId = 1 
                };
            
            kingdomGold.Amount -= buildingPrice;
            kingdom.Buildings.Add(toBeAdded);
            
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
            if (kingdomId == 0 || _applicationContext.Kingdoms
                    .Last().KingdomId > kingdomId)
            {
               throw new KingdomDoesNotExistException();
            }
        }
    }
}