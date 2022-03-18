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
    }
}