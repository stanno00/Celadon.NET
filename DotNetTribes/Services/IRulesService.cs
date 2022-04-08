using DotNetTribes.DTOs;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public interface IRulesService
    {
        public int StartingGold();
        public int StartingFood();
        public int TownhallPrice(int level);
        public int FarmPrice(int level);
        public int MinePrice(int level);
        public int AcademyPrice(int level);
        public int TroopPrice(int level);
        public int MarketplacePrice(int level);
        public int TroopsTrainSpeedPrice(int level);
        public int BuildingBuildSpeedPrice(int level);
        public int MineProduceBonusPrice(int level);
        public int FarmProduceBonusPrice(int level);
        public int AllTroopsDefBonusPrice(int level);
        public int AllTroopsAtkBonusPrice(int level);
        public int MarketplaceTradeAmount(int level);
        public int TownhallHP(int level);
        public int FarmHP(int level);
        public int MineHP(int level);
        public int AcademyHP(int level);
        public int TroopHp(int level);
        public int MarketplaceHP(int level);
        public int TownhallBuildingTime(int level, int kingdomId);
        public int FarmBuildingTime(int level, int kingdomId);
        public int MineBuildingTime(int level, int kingdomId);
        public int AcademyBuildingTime(int level, int kingdomId);
        public int TroopBuildingTime(int level, int kingdomId);
        public int MarketplaceBuildingTime(int level, int kingdomId);
        public int TroopsTrainSpeedTime(int level);
        public int BuildingBuildSpeedTime(int level);
        public int MineProduceBonusTime(int level);
        public int FarmProduceBonusTime(int level);
        public int AllTroopsDefBonusTime(int level);
        public int AllTroopsAtkBonusTime(int level);
        public int StorageLimit(int townhallLevel);
        public int TroopCapacity(int troopLevel);
        public int TroopFoodConsumption(int troopLevel);
        public int TroopAttack(int troopLevel, int kingdomId);
        public int TroopDefense(int troopLevel, int kingdomId);
        public int BuildingResourceGeneration(Building building, int kingdomId);


        public BuildingDetailsDTO GetBuildingDetails(BuildingType type, int level, int kingdomId);
        
        public int MapBoundariesX();
        public int MapBoundariesY();
    }
}