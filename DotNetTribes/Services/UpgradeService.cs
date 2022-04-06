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
                .Include(k => k.BuildingUpgrades)
                .Single(k => k.KingdomId == kingdomId);

            var upgradeName = upgrade.UpgradeName;

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString() ||
                upgradeName == BlacksmithUpgrades.Scout.ToString())
            {
                var result = UpgradingBlacksmith(kingdom, upgradeName);
                return result;
            }

            throw new BuildingCreationException("Upgrade does not exist");
        }

        private BuildingsUpgradesResponseDto UpgradingBlacksmith(Kingdom kingdom, string upgradeName)
        {
            var upgradeToBeAdded = new BuildingUpgrades();

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString())
            {
                upgradeToBeAdded = new BuildingUpgrades()
                {
                    Name = upgradeName,
                    StartedAt = _timeService.GetCurrentSeconds(),
                    FinishedAt = _timeService.GetCurrentSeconds() + _rules.TimeUpgradeForSpecialTroopRanger()
                };
            }

            if (upgradeName == BlacksmithUpgrades.Scout.ToString())
            {
                upgradeToBeAdded = new BuildingUpgrades()
                {
                    Name = upgradeName,
                    StartedAt = _timeService.GetCurrentSeconds(),
                    FinishedAt = _timeService.GetCurrentSeconds() + _rules.TimeUpgradeForSpecialTroopScout()
                };
            }

            if (kingdom.BuildingUpgrades.FirstOrDefault(u => u.Name == upgradeName) != null)
            {
                throw new BuildingCreationException("Building already have this upgrade");
            }

            var gold = kingdom.Resources.Single(r => r.Type == ResourceType.Gold);
            var food = kingdom.Resources.Single(r => r.Type == ResourceType.Food);

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString() && gold.Amount < 2000 ||
                upgradeName == BlacksmithUpgrades.Scout.ToString() && gold.Amount < 1000)
            {
                throw new BuildingCreationException("You don't have enough gold!");
            }

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString() && food.Amount < 2000 ||
                upgradeName == BlacksmithUpgrades.Scout.ToString() && food.Amount < 1000)
            {
                throw new BuildingCreationException("You don't have enough food!");
            }

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString())
            {
                gold.Amount -= _rules.UpgradeForSpecialTroopRanger();
            }

            if (upgradeName == BlacksmithUpgrades.Ranger.ToString())
            {
                food.Amount -= _rules.UpgradeForSpecialTroopRanger();
            }

            if (upgradeName == BlacksmithUpgrades.Scout.ToString())
            {
                gold.Amount -= _rules.UpgradeForSpecialTroopScout();
            }

            if (upgradeName == BlacksmithUpgrades.Scout.ToString())
            {
                food.Amount -= _rules.UpgradeForSpecialTroopScout();
            }

            kingdom.BuildingUpgrades.Add(upgradeToBeAdded);
            _applicationContext.SaveChanges();
            return new BuildingsUpgradesResponseDto()
            {
                Name = upgradeName,
                StartedAt = upgradeToBeAdded.StartedAt,
                FinishedAt = upgradeToBeAdded.FinishedAt,
                KingdomId = kingdom.KingdomId,
            };
        }
    }
}