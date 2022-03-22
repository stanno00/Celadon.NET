﻿namespace DotNetTribes.Models
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
        public int TownhallLevelOneDuration { get; set; }
        public int TownhallLevelNDuration { get; set; }
        public int FarmAllLevelsDuration { get; set; }
        public int FarmAllLevelsFoodGeneration { get; set; }
        public int MineAllLevesDuration { get; set; }
        public int MineALlLevelsGoldGeneration { get; set; }
        public int AcademyLevelOneDuration { get; set; }
        public int AcademyLevelNDuration { get; set; }
        public int TroopAllLevelsDuration { get; set; }
        public int TownhallHP { get; set; }
        public int FarmHP { get; set; }
        public int MineHP { get; set; }
        public int AcademyHP { get; set; }
        public int TroopHP { get; set; }
        public int TroopFoodConsumption { get; set; }
    }
}