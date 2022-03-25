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
        public int TownhallHP(int level);
        public int FarmHP(int level);
        public int MineHP(int level);
        public int AcademyHP(int level);
        public int TroopHp(int level);
        public int TownhallBuildingTime(int level);
        public int FarmBuildingTime(int level);
        public int MineBuildingTime(int level);
        public int AcademyBuildingTime(int level);
        public int TroopBuildingTime(int level);
        public int StorageLimit(int townhallLevel);
        public int TroopCapacity(int troopLevel);
        public int TroopFoodConsumption(int troopLevel);
        public int TroopAttack(int troopLevel);
        public int TroopDefense(int troopLevel);
        public int BuildingResourceGeneration(Building building);


        public BuildingDetailsDTO GetBuildingDetails(BuildingType type, int level);
        
        public int MapBoundariesX();
        public int MapBoundariesY();
    }
}