using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes.Services
{
    public class UpgradeService : IUpgradeService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IRulesService _rules;
        private readonly ITimeService _timeService;


        public UpgradeService(ApplicationContext applicationContext, IRulesService rules, ITimeService timeService)
        {
            _applicationContext = applicationContext;
            _rules = rules;
            _timeService = timeService;
        }

        public BuildingsUpgradesResponseDto AddUpgrade(int kingdomId, BuildingsUpgradesRequestDto upgrade)
        {
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Resources)
                .Include(k => k.Buildings)
                .Include(k => k.BuildingUpgrade)
                .Single(k => k.KingdomId == kingdomId);

            var upgradeName = upgrade.UpgradeName;

            if (upgradeName == AllBuildingUpgrades.Ranger ||
                upgradeName == AllBuildingUpgrades.Scout)
            {
                var result = UpgradingBlacksmith(kingdom, upgrade);
                return result;
            }

            throw new BuildingCreationException("Upgrade does not exist");
        }

        private BuildingsUpgradesResponseDto UpgradingBlacksmith(Kingdom kingdom, BuildingsUpgradesRequestDto upgrade)
        {
            var upgradeToBeAdded = new BuildingUpgrade();
            var upgradeName = upgrade.UpgradeName;

            switch (upgradeName)
            {
                case AllBuildingUpgrades.Ranger:
                    upgradeToBeAdded = new BuildingUpgrade()
                    {
                        Name = upgradeName,
                        StartedAt = _timeService.GetCurrentSeconds(),
                        FinishedAt = _timeService.GetCurrentSeconds() + _rules.TimeUpgradeForSpecialTroopRanger()
                    };
                    break;
                case AllBuildingUpgrades.Scout:
                    upgradeToBeAdded = new BuildingUpgrade()
                    {
                        Name = upgradeName,
                        StartedAt = _timeService.GetCurrentSeconds(),
                        FinishedAt = _timeService.GetCurrentSeconds() + _rules.TimeUpgradeForSpecialTroopScout()
                    };
                    break;
            }

            if (kingdom.BuildingUpgrade.FirstOrDefault(u => u.Name == upgradeName) != null)
            {
                throw new BuildingCreationException("Building already have this upgrade");
            }

            var gold = kingdom.Resources.Single(r => r.Type == ResourceType.Gold);
            var food = kingdom.Resources.Single(r => r.Type == ResourceType.Food);

            if (upgradeName == AllBuildingUpgrades.Ranger && gold.Amount < 2000 ||
                upgradeName == AllBuildingUpgrades.Scout && gold.Amount < 1000)
            {
                throw new BuildingCreationException("You don't have enough gold!");
            }

            if (upgradeName == AllBuildingUpgrades.Ranger && food.Amount < 2000 ||
                upgradeName == AllBuildingUpgrades.Scout && food.Amount < 1000)
            {
                throw new BuildingCreationException("You don't have enough food!");
            }

            if (upgradeName == AllBuildingUpgrades.Ranger)
            {
                gold.Amount -= _rules.UpgradeForSpecialTroopRanger();
                food.Amount -= _rules.UpgradeForSpecialTroopRanger();
            }

            if (upgradeName == AllBuildingUpgrades.Scout)
            {
                gold.Amount -= _rules.UpgradeForSpecialTroopScout();
                food.Amount -= _rules.UpgradeForSpecialTroopScout();
            }

            kingdom.BuildingUpgrade.Add(upgradeToBeAdded);
            _applicationContext.SaveChanges();
            return new BuildingsUpgradesResponseDto()
            {
                Name = upgradeName.ToString(),
                StartedAt = upgradeToBeAdded.StartedAt,
                FinishedAt = upgradeToBeAdded.FinishedAt,
                KingdomId = kingdom.KingdomId,
            };
        }
    }
}