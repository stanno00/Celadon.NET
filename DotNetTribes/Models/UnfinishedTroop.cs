namespace DotNetTribes.Models
{
    public class UnfinishedTroop
    {
        public long UnfinishedTroopId { get; set; }
        public int KingdomId { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public long UpdatedAt { get; set; }
        
        public int Level { get; set; }
        public bool Upgrading { get; set; }
    }
}