using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class RulesService : IRulesService
    {
        private readonly GameRules _r;
        private readonly ApplicationContext _applicationContext;

        public RulesService(ApplicationContext applicationContext)
        {
            _r = applicationContext.GameRules.FirstOrDefault(r => r.Name == "Production");
            _applicationContext = applicationContext;
        }

        public int StartingGold()
        {
            return _r.StartingGold;
        }

        public int StartingFood()
        {
            return _r.StartingFood;
        }

        public int TownhallPrice(int level)
        {
            return level * _r.TownhallAllLevelsCost;
        }

        public int FarmPrice(int level)
        {
            return level * _r.FarmAllLevelsCost;
        }

        public int MinePrice(int level)
        {
            return level * _r.MineAllLevelsCost;
        }

        public int AcademyPrice(int level)
        {
            if (level == 1)
            {
                return _r.AcademyLevelOneCost;
            }

            return level * _r.AcademyLevelNCost;
        }

        public int TroopPrice(int level)
        {
            return level * _r.TroopAllLevelsCost;
        }

        public int MarketplacePrice(int level)
        {
            if (level == 1)
            {
                return _r.MarketplaceLevelOneCost;
            }

            return level * _r.MarketplaceAllLevelsCost;
        }

        public int TroopsTrainSpeedPrice(int level)
        {
            if (level == 0)
            {
                return _r.TroopsTrainSpeedCost;
            }
            return level * _r.TroopsTrainSpeedCost;
        }

        public int BuildingBuildSpeedPrice(int level)
        {
            if (level == 0)
            {
                return _r.BuildingBuildSpeedCost;
            }
            return level * _r.BuildingBuildSpeedCost;
        }

        public int MineProduceBonusPrice(int level)
        {
            if (level == 0)
            {
                return _r.MineProduceBonusCost;
            }
            return level * _r.MineProduceBonusCost;
        }

        public int FarmProduceBonusPrice(int level)
        {
            if (level == 0)
            {
                return _r.FarmProduceBonusCost;
            }
            return level * _r.FarmProduceBonusCost;
        }

        public int AllTroopsDefBonusPrice(int level)
        {
            if (level == 0)
            {
                return _r.AllTroopsDefBonusCost;
            }
            return level * _r.AllTroopsDefBonusCost;
        }

        public int AllTroopsAtkBonusPrice(int level)
        {
            if (level == 0)
            {
                return _r.AllTroopsAtkBonusCost;
            }
            return level * _r.AllTroopsAtkBonusCost;
        }

        public int UniversityPrice(int level)
        {
            return level * _r.UniversityAllLevelCost;
        }

        public int MarketplaceTradeAmount(int level)
        {
            return level * _r.MarketplaceMaxResources;
        }

        public int TownhallHP(int level)
        {
            return level * _r.TownhallHP;
        }

        public int FarmHP(int level)
        {
            return level * _r.FarmHP;
        }

        public int MineHP(int level)
        {
            return level * _r.MineHP;
        }

        public int AcademyHP(int level)
        {
            return level * _r.AcademyHP;
        }

        public int TroopHp(int level)
        {
            return level * _r.TroopHP;
        }

        public int MarketplaceHP(int level)
        {
            return level * _r.MarketplaceHP;
        }

        public int UniversityHP(int level)
        {
            return level * _r.UniversityHP;
        }

        public int TownhallBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            if (level == 1)
            {
                return _r.TownhallLevelOneDuration;
            }

            return Convert.ToInt32(level * _r.TownhallLevelNDuration * timeReduction);
        }

        public int FarmBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            return Convert.ToInt32(level * _r.FarmAllLevelsDuration * timeReduction);
        }

        public int MineBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            return Convert.ToInt32(level * _r.MineAllLevesDuration * timeReduction);
        }

        public int AcademyBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            if (level == 1)
            {
                return _r.AcademyLevelOneDuration;
            }

            return Convert.ToInt32(level * _r.AcademyLevelNDuration * timeReduction);
        }

        public int TroopBuildingTime(int level, int kingdomId)
        {
            double timeReduction = 1;
            var trainingSpeedUpgrade = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.TroopsTrainSpeed);
            if (trainingSpeedUpgrade != null)
            {
                timeReduction = 1 - trainingSpeedUpgrade.EffectStrength * trainingSpeedUpgrade.Level;
            }
            return Convert.ToInt32(level * _r.TroopAllLevelsDuration * timeReduction);
        }

        public int MarketplaceBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            if (level == 1)
            {
                return Convert.ToInt32(_r.MarketplaceLevelOneDuration * timeReduction);
            }

            return Convert.ToInt32(level * _r.MarketplaceAllLevelsDuration * timeReduction);
        }

        public int TroopsTrainSpeedTime(int level)
        {
            if (level == 0)
            {
                return _r.TroopsTrainSpeedDuration;
            }
            return level * _r.TroopsTrainSpeedDuration;
        }

        public int BuildingBuildSpeedTime(int level)
        {
            if (level == 0)
            {
                return _r.BuildingBuildSpeedDuration;
            }
            return level * _r.BuildingBuildSpeedDuration;
        }

        public int MineProduceBonusTime(int level)
        {
            if (level == 0)
            {
                return _r.MineProduceBonusDuration;
            }
            return level * _r.MineProduceBonusDuration;
        }

        public int FarmProduceBonusTime(int level)
        {
            if (level == 0)
            {
                return _r.FarmProduceBonusDuration;
            }
            return level * _r.FarmProduceBonusDuration;
        }

        public int AllTroopsDefBonusTime(int level)
        {
            if (level == 0)
            {
                return _r.AllTroopsDefBonusDuration;
            }
            return level * _r.AllTroopsDefBonusDuration;
        }

        public int AllTroopsAtkBonusTime(int level)
        {
            if (level == 0)
            {
                return _r.AllTroopsAtkBonusDuration;
            }
            return level * _r.AllTroopsAtkBonusDuration;
        }

        public int UniversityBuildingTime(int level, int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            
            return Convert.ToInt32(level * _r.UniversityAllLevelDuration * timeReduction);
        }

        public int StorageLimit(int townhallLevel)
        {
            return townhallLevel * _r.StorageLimit;
        }

        public int TroopCapacity(int troopLevel)
        {
            return troopLevel * _r.TroopCapacity;
        }

        public int TroopFoodConsumption(int troopLevel)
        {
            return troopLevel * _r.TroopFoodConsumption;
        }

        public int TroopAttack(int troopLevel, int kingdomId)
        {
            int attackBonus = 0;
            var troopAtkBonus = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.AllTroopsAtkBonus);
            if (troopAtkBonus != null)
            {
                attackBonus = Convert.ToInt32(troopAtkBonus.EffectStrength * troopAtkBonus.Level);
            }
            
            return troopLevel * _r.TroopAttack + attackBonus;
        }

        public int TroopDefense(int troopLevel, int kingdomId)
        {
            int defenseBonus = 0;
            var troopDefBonus = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.AllTroopsDefBonus);
            if (troopDefBonus != null)
            {
                defenseBonus = Convert.ToInt32(troopDefBonus.EffectStrength * troopDefBonus.Level);
            }
            
            return troopLevel * _r.TroopDefense + defenseBonus;
        }

        public int MapBoundariesX()
        {
            return _r.MapBoundariesX;
        }

        public int MapBoundariesY()
        {
            return _r.MapBoundariesY;
        }
        
        public int BlacksmithPrice()
        {
            return _r.BlacksmithLevelOneCost;
        }

        public int BlacksmithHp()
        {
            return _r.BlacksmithHp;
        }

        public int BlacksmithBuildingTime(int kingdomId)
        {
            double timeReduction = GetBuildTimeReduction(kingdomId);
            return Convert.ToInt32(_r.BlacksmithLevelOneDuration * timeReduction);
        }

        public int TrainingTimeSpecialTroopRanger(int kingdomId)
        {
            double timeReduction = 1;
            var trainingSpeedUpgrade = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.TroopsTrainSpeed);
            if (trainingSpeedUpgrade != null)
            {
                timeReduction = 1 - trainingSpeedUpgrade.EffectStrength * trainingSpeedUpgrade.Level;
            }
            return Convert.ToInt32(_r.TrainingTimeTroopRanger * timeReduction);
        }

        public int TrainingTimeSpecialTroopScout(int kingdomId)
        {
            double timeReduction = 1;
            var trainingSpeedUpgrade = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.TroopsTrainSpeed);
            if (trainingSpeedUpgrade != null)
            {
                timeReduction = 1 - trainingSpeedUpgrade.EffectStrength * trainingSpeedUpgrade.Level;
            }
            return Convert.ToInt32(_r.TrainingTimeTroopScout * timeReduction);
        }

        public int CostSpecialTroopScout(int level)
        {
            return level * _r.CostTroopScout;
        }

        public int CostSpecialTroopRanger(int level)
        {
            return level * _r.CostTroopRanger;
        }

        public int UpgradeForSpecialTroopRanger()
        {
            return _r.UpgradeForTroopRanger;
        }

        public int UpgradeForSpecialTroopScout()
        {
            return _r.UpgradeForSpecialTroopScout;
        }

        public int SpecialTroopRangerHp()
        {
            return _r.TroopRangerHp;
        }

        public int SpecialTroopScoutHp()
        {
            return _r.TroopScoutHp;
        }

        public int TimeUpgradeForSpecialTroopScout()
        {
            return _r.TimeUpgradeTroopScout;
        }

        public int TimeUpgradeForSpecialTroopRanger()
        {
            return _r.TimeUpgradeTroopRanger;
        }

        public int IronMineBuildingTime(int level)
        {
            return 300 + _r.IronMineDuration * level;
        }

        public int IronMinePrice(int level)
        {
            return 500 + _r.IronMineCost * level;
        }

        public int IronMineHP(int level)
        {
            return 300 + 100 * level;
        }

        public int BuildingResourceGeneration(Building building, int kingdomId)
        {
            double productionBoost = 1;

            var resourceGeneration = 0;
            switch (building.Type)
            {
                case BuildingType.Mine:
                    resourceGeneration = _r.MineALlLevelsGoldGeneration;
                    var mineProduceBonus = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                        u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.MineProduceBonus);
                    if (mineProduceBonus != null)
                    {
                        productionBoost = 1 + mineProduceBonus.EffectStrength * mineProduceBonus.Level;
                    }
                    break;
                case BuildingType.Farm:
                    resourceGeneration = _r.FarmAllLevelsFoodGeneration;
                    var farmProduceBonus = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                        u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.FarmProduceBonus);
                    if (farmProduceBonus != null)
                    {
                        productionBoost = 1 + farmProduceBonus.EffectStrength * farmProduceBonus.Level;
                    }
                    break;
                case BuildingType.IronMine:
                    resourceGeneration = _r.IronMineGeneration;
                    break;
            }

            return Convert.ToInt32((building.Level * resourceGeneration + 5) * productionBoost);
        }

        public BuildingDetailsDTO GetBuildingDetails(BuildingType type, int level, int kingdomId)
        {
            switch (type)
            {
                case BuildingType.TownHall:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = TownhallPrice(level),
                        BuildingHP = TownhallHP(level),
                        BuildingDuration = TownhallBuildingTime(level, kingdomId)
                    };
                case BuildingType.Farm:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = FarmPrice(level),
                        BuildingHP = FarmHP(level),
                        BuildingDuration = FarmBuildingTime(level, kingdomId)
                    };
                case BuildingType.Mine:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = MinePrice(level),
                        BuildingHP = MineHP(level),
                        BuildingDuration = MineBuildingTime(level, kingdomId)
                    };
                case BuildingType.Academy:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = AcademyPrice(level),
                        BuildingHP = AcademyHP(level),
                        BuildingDuration = AcademyBuildingTime(level, kingdomId)
                    };
                case BuildingType.Marketplace:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = MarketplacePrice(level),
                        BuildingHP = MarketplaceHP(level),
                        BuildingDuration = MarketplaceBuildingTime(level, kingdomId)
                    };
                case BuildingType.University:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = UniversityPrice(level),
                        BuildingHP = UniversityHP(level),
                        BuildingDuration = UniversityBuildingTime(level, kingdomId)
                    };
                case BuildingType.Blacksmith:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = BlacksmithPrice(),
                        BuildingHP = BlacksmithHp(),
                        BuildingDuration = BlacksmithBuildingTime(kingdomId)
                    };
                case BuildingType.IronMine:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = IronMinePrice(level),
                        BuildingHP = IronMineHP(level),
                        BuildingDuration = IronMineBuildingTime(level)
                    };
                default:
                    throw new BuildingCreationException(
                        "This should not happen under any circumstances. Recommended actions:" +
                        "Troubleshooting, exorcism of the server, throwing heavy objects at high velocities in the direction of" +
                        "the developer's head.");
            }
        }

        private double GetBuildTimeReduction(int kingdomId)
        {
            double timeReduction = 1;
            var buildSpeedUpgrade = _applicationContext.UniversityUpgrades.FirstOrDefault(u =>
                u.KingdomId == kingdomId && u.UpgradeType == UpgradeType.BuildingBuildSpeed);
            if (buildSpeedUpgrade != null)
            {
                timeReduction = 1 - buildSpeedUpgrade.EffectStrength * buildSpeedUpgrade.Level;
            }

            return timeReduction;
        }
    }
}