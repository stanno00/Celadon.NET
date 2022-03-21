namespace DotNetTribes.Models
{
    public class Troop
    {
        public long TroopId { get; set; }
        public int KingdomId { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public bool ConsumingFood { get; set; }

    }
}