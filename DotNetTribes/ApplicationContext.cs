using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Troop> Troops { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<GameRules> GameRules { get; set; }
        
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<BuildingUpgrades> BuildingUpgrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameRules>().HasData(
                new GameRules
                {
                    GameRulesId = 1,
                    Name = "Production",
                    StartingGold = 500,
                    StartingFood = 500,
                    TownhallAllLevelsCost = 200,
                    FarmAllLevelsCost = 100,
                    MineAllLevelsCost = 100,
                    AcademyLevelOneCost = 150,
                    AcademyLevelNCost = 100,
                    TroopAllLevelsCost = 25,
                    MarketplaceLevelOneCost = 1,
                    MarketplaceAllLevelsCost = 100,
                    TownhallLevelOneDuration = 120,
                    TownhallLevelNDuration = 60,
                    FarmAllLevelsDuration = 60,
                    FarmAllLevelsFoodGeneration = 5,
                    MineAllLevesDuration = 60,
                    MineALlLevelsGoldGeneration = 5,
                    AcademyLevelOneDuration = 90,
                    AcademyLevelNDuration = 60,
                    TroopAllLevelsDuration = 30,
                    MarketplaceLevelOneDuration = 1,
                    MarketplaceAllLevelsDuration = 10,
                    MarketplaceMaxResources = 75,
                    TownhallHP = 200,
                    FarmHP = 100,
                    MineHP = 100,
                    AcademyHP = 150,
                    TroopHP = 20,
                    MarketplaceHP = 100,
                    TroopFoodConsumption = 2,
                    TroopAttack = 10,
                    TroopDefense = 5,
                    TroopCapacity = 2,
                    StorageLimit = 100,
                    MapBoundariesX = 101,
                    MapBoundariesY = 101,
                    BlacksmithLevelOneCost = 500,
                    BlacksmithHp = 150,
                    BlacksmithLevelOneDuration = 600,
                    UpgradeForSpecialTroopScout = 1000,
                    TimeUpgradeForSpecialTroopScout = 72000,
                    TimeUpgradeForSpecialTroopRanger = 43200,
                    TrainingTimeSpecialTroopRanger = 200,
                    UpgradeForSpecialTroopRanger = 2000,
                    CostSpecialTroopRanger = 50,
                    SpecialTroopRangerHp = 10,
                    TrainingTimeSpecialTroopScout = 300,
                    SpecialTroopScoutHp = 1,
                    CostSpecialTroopScout = 100,

                });
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
    }
}