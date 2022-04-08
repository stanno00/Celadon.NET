namespace DotNetTribes.Models
{
    public class GameRules
    {
        public int GameRulesId { get; set; }
        public string Name { get; set; }
        public int StartingGold { get; set; }
        public int StartingFood { get; set; }
        public int TownhallAllLevelsCost { get; set; }
        public int FarmAllLevelsCost { get; set; }
        public int MineAllLevelsCost { get; set; }
        public int AcademyLevelOneCost { get; set; }
        public int AcademyLevelNCost { get; set; }
        public int TroopAllLevelsCost { get; set; }
        public int MarketplaceAllLevelsCost { get; set; }
        public int MarketplaceLevelOneCost { get; set; }
        public int UniversityAllLevelCost { get; set; }
        public int TroopsTrainSpeedCost { get; set; }
        public int BuildingBuildSpeedCost { get; set; }
        public int MineProduceBonusCost { get; set; }
        public int FarmProduceBonusCost { get; set; }
        public int AllTroopsDefBonusCost { get; set; }
        public int AllTroopsAtkBonusCost { get; set; }
        public int TownhallLevelOneDuration { get; set; }
        public int TownhallLevelNDuration { get; set; }
        public int FarmAllLevelsDuration { get; set; }
        public int FarmAllLevelsFoodGeneration { get; set; }
        public int MineAllLevesDuration { get; set; }
        public int MineALlLevelsGoldGeneration { get; set; }
        public int MarketplaceAllLevelsDuration { get; set; }
        public int MarketplaceLevelOneDuration { get; set; }
        public int MarketplaceMaxResources { get; set; }
        public int AcademyLevelOneDuration { get; set; }
        public int AcademyLevelNDuration { get; set; }
        public int TroopAllLevelsDuration { get; set; }
        public int UniversityAllLevelDuration { get; set; }
        public int TroopsTrainSpeedDuration { get; set; }
        public int BuildingBuildSpeedDuration { get; set; }
        public int MineProduceBonusDuration { get; set; }
        public int FarmProduceBonusDuration { get; set; }
        public int AllTroopsDefBonusDuration { get; set; }
        public int AllTroopsAtkBonusDuration { get; set; }
        public int TownhallHP { get; set; }
        public int StorageLimit { get; set; }
        public int FarmHP { get; set; }
        public int MineHP { get; set; }
        public int AcademyHP { get; set; }
        public int TroopHP { get; set; }
        public int MarketplaceHP { get; set; }
        public int UniversityHP { get; set; }
        public int TroopFoodConsumption { get; set; }
        
        public int TroopAttack { get; set; }
        public int TroopDefense { get; set; }
        public int TroopCapacity { get; set; }

        public int MapBoundariesX { get; set; }
        public int MapBoundariesY { get; set; }

    }
}