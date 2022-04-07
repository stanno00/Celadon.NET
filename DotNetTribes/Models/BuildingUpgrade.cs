
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class BuildingUpgrade
    {
        public int BuildingUpgradeId { get; set; }
        public AllBuildingUpgrades Name { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public int KingdomId { get; set; }
    }
}