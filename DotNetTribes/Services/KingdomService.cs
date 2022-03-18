using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IResourceService _resourceService;
        private readonly ITimeService _timeService;
        private readonly IRulesService _rules;

        public KingdomService(ApplicationContext applicationContext, IResourceService resourceService,
            ITimeService timeService, IRulesService rules)
        {
            _applicationContext = applicationContext;
            _resourceService = resourceService;
            _timeService = timeService;
            _rules = rules;
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
                Resources = _resourceService.GetKingdomResources(kingdomId),
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

            if (string.IsNullOrEmpty(request.Type))
            {
                throw new BuildingCreationException("Building type required.");
            }

            if (!Enum.TryParse(request.Type, true, out BuildingType requestedBuilding))
            {
                throw new BuildingCreationException("Incorrect building type.");
            }

            int buildingPrice;
            Building toBeAdded;

            if (requestedBuilding == BuildingType.TownHall)
            {
                buildingPrice = _rules.TownhallPrice(1);
                toBeAdded = new Building
                {
                    Type = BuildingType.TownHall,
                    Level = 1,
                    Hp = _rules.TownhallHP(1),
                    Started_at = _timeService.GetCurrentSeconds(),
                    Finished_at = _timeService.GetCurrentSeconds() + _rules.TownhallBuildingTime(1),
                    KingdomId = kingdomId
                };
            }
            else if (requestedBuilding == BuildingType.Farm)
            {
                buildingPrice = _rules.FarmPrice(1);
                toBeAdded = new Building
                {
                    Type = BuildingType.Farm,
                    Level = 1,
                    Hp = _rules.FarmHP(1),
                    Started_at = _timeService.GetCurrentSeconds(),
                    Finished_at = _timeService.GetCurrentSeconds() + _rules.FarmBuildingTime(1),
                    KingdomId = kingdomId
                };
            }
            else if (requestedBuilding == BuildingType.Mine)
            {
                buildingPrice = _rules.MinePrice(1);
                toBeAdded = new Building
                {
                    Type = BuildingType.Mine,
                    Level = 1,
                    Hp = _rules.MineHP(1),
                    Started_at = _timeService.GetCurrentSeconds(),
                    Finished_at = _timeService.GetCurrentSeconds() + _rules.MineBuildingTime(1),
                    KingdomId = kingdomId
                };
            }
            else
            {
                buildingPrice = _rules.AcademyPrice(1);
                toBeAdded = new Building
                {
                    Type = BuildingType.Academy,
                    Level = 1,
                    Hp = _rules.AcademyHP(1),
                    Started_at = _timeService.GetCurrentSeconds(),
                    Finished_at = _timeService.GetCurrentSeconds() + _rules.AcademyBuildingTime(1),
                    KingdomId = kingdomId
                };
            }

            if (buildingPrice > kingdomGold.Amount)
            {
                throw new BuildingCreationException("Gold needed.");
            }

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