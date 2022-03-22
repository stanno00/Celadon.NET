using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class BuildingsService : IBuildingsService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ITimeService _timeService;
        private readonly IRulesService _rules;

        public BuildingsService(ApplicationContext applicationContext, ITimeService timeService, IRulesService rules)
        {
            _applicationContext = applicationContext;
            _timeService = timeService;
            _rules = rules;
        }


        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Type))
            {
                throw new BuildingCreationException("Building type required.");
            }

            if (!Enum.TryParse(request.Type, true, out BuildingType requestedBuilding))
            {
                throw new BuildingCreationException("Incorrect building type.");
            }

            var kingdom = _applicationContext.Kingdoms
                .Include(b => b.Buildings)
                .Include(r => r.Resources)
                .FirstOrDefault(k => k.KingdomId == kingdomId);

            var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type.Equals(ResourceType.Gold));

            BuildingDetailsDTO buildingDetails = _rules.GetBuildingDetails(requestedBuilding, 1);
            Building toBeAdded =
                CreateNewBuilding(kingdomId, buildingDetails.BuildingHP, buildingDetails.BuildingDuration, requestedBuilding);


            if (buildingDetails.BuildingPrice > kingdomGold.Amount)
            {
                throw new BuildingCreationException("Gold needed.");
            }

            kingdomGold.Amount -= buildingDetails.BuildingPrice;
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

        private Building CreateNewBuilding(int kingdomId, int hp, int duration, BuildingType type)
        {
            Building newBuilding = new Building
            {
                KingdomId = kingdomId,
                Level = 1,
                Type = type,
                Started_at = _timeService.GetCurrentSeconds(),
                Finished_at = _timeService.GetCurrentSeconds() + duration,
                Hp = hp
            };
            return newBuilding;
        }

        public BuildingResponseDTO UpgradeBuilding(int kingdomId, int buildingId)
        {
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Resources)
                .Include(k => k.Buildings)
                .Single(k => k.KingdomId == kingdomId);
            
            var buildingToBeUpgraded = kingdom.Buildings.FirstOrDefault(b => b.BuildingId == buildingId);
            
            if (buildingToBeUpgraded == null)
            {
                throw new BuildingCreationException("Building does not exist!");
            }

            var townHallLevel = kingdom.Buildings.First(b => b.Type == BuildingType.TownHall).Level;
            var buildingType = buildingToBeUpgraded.Type;
            var buildingLevel = buildingToBeUpgraded.Level;
            var buildingNextLevel = buildingLevel + 1;
            
            if (buildingLevel >= townHallLevel && buildingType != BuildingType.TownHall)
            {
                throw new BuildingCreationException("Townhall level is too low!");
            }

            var upgradeBuilding = _rules.GetBuildingDetails(buildingType, buildingNextLevel);

            var gold = kingdom.Resources.Single(r => r.Type == ResourceType.Gold);
            var food = kingdom.Resources.Single(r => r.Type == ResourceType.Food);

            if (gold.Amount < upgradeBuilding.BuildingPrice)
            {
                throw new BuildingCreationException("You dont have enough gold!");
            }

            gold.Amount -= upgradeBuilding.BuildingPrice;
            buildingToBeUpgraded.Hp = upgradeBuilding.BuildingHP;
            buildingToBeUpgraded.Level = buildingNextLevel;
            buildingToBeUpgraded.Started_at = _timeService.GetCurrentSeconds();
            buildingToBeUpgraded.Finished_at = buildingToBeUpgraded.Started_at + upgradeBuilding.BuildingDuration;

            switch (buildingToBeUpgraded.Type)
            {
                case BuildingType.Mine:
                    gold.UpdatedAt = buildingToBeUpgraded.Finished_at;
                    break;
                case BuildingType.Farm:
                    food.UpdatedAt = buildingToBeUpgraded.Finished_at;
                    break;
            }

            _applicationContext.SaveChanges();

            var response = new BuildingResponseDTO()
            {
                Id = buildingId,
                Type = buildingToBeUpgraded.Type.ToString(),
                Level = buildingNextLevel,
                Hp = buildingToBeUpgraded.Hp,
                Started_at = buildingToBeUpgraded.Started_at.ToString(),
                Finished_at = buildingToBeUpgraded.Finished_at.ToString()
            };

            return response;
        }
    }
}