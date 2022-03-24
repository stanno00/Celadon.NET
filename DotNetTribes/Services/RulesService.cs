using System;
using System.Linq;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class RulesService: IRulesService
    {
        private readonly ApplicationContext _applicationContext;
        private GameRules _r;

        public RulesService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _r = _applicationContext.GameRules.FirstOrDefault(r => r.Name == "Production");
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
        
        public int TownhallBuildingTime(int level)
        {
            if (level == 1)
            {
                return _r.TownhallLevelOneDuration;
            }

            return level * _r.TownhallLevelNDuration;
        }
        
        public int FarmBuildingTime(int level)
        {
            return level * _r.FarmAllLevelsDuration;
        }
        
        public int MineBuildingTime(int level)
        {
            return level * _r.MineAllLevesDuration;
        }
        
        public int AcademyBuildingTime(int level)
        {
            if (level == 1)
            {
                return _r.AcademyLevelOneDuration;
            }

            return level * _r.AcademyLevelNDuration;
        }
        
        public int TroopBuildingTime(int level)
        {
            return level * _r.TroopAllLevelsDuration;
        }

        public int MarketplaceBuildingTime(int level)
        {
            if (level == 1)
            {
                return _r.MarketplaceLevelOneDuration;
            }

            return level * _r.MarketplaceAllLevelsDuration;
        }

        public int BuildingResourceGeneration(Building building)
        {
            var resourceGeneration = 0;
            switch (building.Type)
            {
                case BuildingType.Mine:
                    resourceGeneration = _r.MineALlLevelsGoldGeneration;
                    break;
                case BuildingType.Farm:
                    resourceGeneration = _r.FarmAllLevelsFoodGeneration;
                    break;
            }

            return building.Level * resourceGeneration + 5;
        }

        public BuildingDetailsDTO GetBuildingDetails(BuildingType type, int level)
        {
            switch (type)
            {
                case BuildingType.TownHall:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = TownhallPrice(level),
                        BuildingHP = TownhallHP(level),
                        BuildingDuration = TownhallBuildingTime(level)
                    };
                case BuildingType.Farm:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = FarmPrice(level),
                        BuildingHP = FarmHP(level),
                        BuildingDuration = FarmBuildingTime(level)
                    };
                case BuildingType.Mine:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = MinePrice(level),
                        BuildingHP = MineHP(level),
                        BuildingDuration = MineBuildingTime(level)
                    };
                case BuildingType.Academy:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = AcademyPrice(level),
                        BuildingHP = AcademyHP(level),
                        BuildingDuration = AcademyBuildingTime(level)
                    };
                case BuildingType.Marketplace:
                    return new BuildingDetailsDTO
                    {
                        BuildingPrice = MarketplacePrice(level),
                        BuildingHP = MarketplaceHP(level),
                        BuildingDuration = MarketplaceBuildingTime(level)
                    };
                default:
                    throw new BuildingCreationException("This should not happen under any circumstances. Recommended actions:" +
                                                        "Troubleshooting, exorcism of the server, throwing heavy objects at high velocities in the direction of" +
                                                        "the developer's head.");
            }
        }
    }
}