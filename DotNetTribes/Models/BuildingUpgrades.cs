
namespace DotNetTribes.Models
{
    public class BuildingUpgrades
    {
        public int BuildingUpgradesId { get; set; }
        public string Name { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public int KingdomId { get; set; }
    }
}