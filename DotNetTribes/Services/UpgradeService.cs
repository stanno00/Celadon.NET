using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
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

        public UniversityUpgrade BuyUniversityUpgrade(int kingdomId, UpgradeType upgradeType)
        {
            Kingdom kingdom = _applicationContext.Kingdoms
                .Include(k => k.Resources)
                .Include(k => k.Buildings)
                .Include(k => k.Upgrades)
                .Single(k => k.KingdomId == kingdomId);
            
            var kingdomUniversity = kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.University);
            if (kingdomUniversity == null)
            {
                throw new UpgradeException("You don't have a University");
            }

            var upgradeInProgress = kingdom.Upgrades.FirstOrDefault(u => u.FinishedAt > _timeService.GetCurrentSeconds());
            if (upgradeInProgress != null)
            {
                throw new UpgradeException("There is an upgrade in progress");
            }

            UniversityUpgrade theUpgrade = null;
            
            if (upgradeType == UpgradeType.BuildingBuildSpeed)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.BuildingBuildSpeedPrice(upgradeToLvl);
                long researchTime = _rules.BuildingBuildSpeedTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 0.05, resourcesNeeded, upgradeToLvl, researchTime);
                

            }
            if (upgradeType == UpgradeType.FarmProduceBonus)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.FarmProduceBonusPrice(upgradeToLvl);
                long researchTime = _rules.FarmProduceBonusTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 0.05, resourcesNeeded, upgradeToLvl, researchTime);
            }
            if (upgradeType == UpgradeType.MineProduceBonus)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.MineProduceBonusPrice(upgradeToLvl);
                long researchTime = _rules.MineProduceBonusTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 0.05, resourcesNeeded, upgradeToLvl, researchTime);
            }
            if (upgradeType == UpgradeType.TroopsTrainSpeed)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.TroopsTrainSpeedPrice(upgradeToLvl);
                long researchTime = _rules.TroopsTrainSpeedTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 0.05, resourcesNeeded, upgradeToLvl, researchTime);
            }
            if (upgradeType == UpgradeType.AllTroopsAtkBonus)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.AllTroopsAtkBonusPrice(upgradeToLvl);
                long researchTime = _rules.AllTroopsAtkBonusTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 3, resourcesNeeded, upgradeToLvl, researchTime);
            }
            if (upgradeType == UpgradeType.AllTroopsDefBonus)
            {
                int upgradeToLvl = 0;
                var currentLvlUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                if (currentLvlUpgrade != null)
                {
                    upgradeToLvl = currentLvlUpgrade.Level + 1;
                }
                int resourcesNeeded = _rules.AllTroopsDefBonusPrice(upgradeToLvl);
                long researchTime = _rules.AllTroopsDefBonusTime(upgradeToLvl) - GetUniversityLevelTimeReduction(kingdomUniversity);
                theUpgrade = StartUpgradingCheckResources(kingdom, upgradeType, 2, resourcesNeeded, upgradeToLvl, researchTime);
            }
            
            _applicationContext.SaveChanges();
            return theUpgrade;
        }

        private UniversityUpgrade StartUpgradingCheckResources(Kingdom kingdom, UpgradeType upgradeType, double effectStrength, int resourcesNeeded, int upgradeTotLvl, long researchTime)
        {
            if (upgradeTotLvl == 0)
            {
                bool enoughResources = CheckResourcesForUpgrade(kingdom, resourcesNeeded);
                UniversityUpgrade theUpgrade = new UniversityUpgrade()
                {
                    UpgradeType = upgradeType,
                    EffectStrength = effectStrength,
                    Level = 0,
                    KingdomId = kingdom.KingdomId,
                    StartedAt = _timeService.GetCurrentSeconds(),
                    FinishedAt = _timeService.GetCurrentSeconds() + researchTime,
                    AddedToKingdom = false
                };
                _applicationContext.Add(theUpgrade);
                return theUpgrade;
            }
            if (upgradeTotLvl < 6)
            {
                bool enoughResources = CheckResourcesForUpgrade(kingdom, resourcesNeeded);
                var existingUpgrade = kingdom.Upgrades.FirstOrDefault(u => u.UpgradeType == upgradeType);
                existingUpgrade.StartedAt = _timeService.GetCurrentSeconds();
                existingUpgrade.FinishedAt = _timeService.GetCurrentSeconds() + researchTime;
                existingUpgrade.AddedToKingdom = false;
                return existingUpgrade;
            }
            else
            {
                throw new UpgradeException($"You have max level of {upgradeType}");
            }
        }

        private bool CheckResourcesForUpgrade(Kingdom kingdom, int resourcesNeeded)
        {
            if (kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Food)!.Amount > resourcesNeeded && 
                kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold)!.Amount > resourcesNeeded)
            {
                Resource food = kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Food);
                food.Amount -= resourcesNeeded;
                Resource gold = kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold);
                gold.Amount -= resourcesNeeded;
            }
            else
            {
                throw new UpgradeException("Not enough resources");
            }

            return true;
        }

        private long GetUniversityLevelTimeReduction(Building hasUniversity)
        {
            if (hasUniversity.Level == 1)
            {
                return 0;
            }
            return hasUniversity.Level * 1; //have to change after testing
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

        public void ApplyUpgradesWhenFinished()
        {
            var finishedUpgrades = _applicationContext.UniversityUpgrades.Where(u =>
                u.FinishedAt < _timeService.GetCurrentSeconds() && u.AddedToKingdom == false).ToList();

            foreach (var i in finishedUpgrades)
            {
                if (i.UpgradeType == UpgradeType.AllTroopsAtkBonus)
                {
                    var troopToBeUpgraded = _applicationContext.Troops.Where(k => k.KingdomId == i.KingdomId).ToList();
                    foreach (var y in troopToBeUpgraded)
                    {
                        y.Attack += 3;
                    }
                }
                if (i.UpgradeType == UpgradeType.AllTroopsDefBonus)
                {
                    var troopToBeUpgraded = _applicationContext.Troops.Where(k => k.KingdomId == i.KingdomId).ToList();
                    foreach (var y in troopToBeUpgraded)
                    {
                        y.Defense += 2;
                    }
                }
                i.Level += 1;
                i.AddedToKingdom = true;
            }

            _applicationContext.SaveChanges();
        }
    }
}