namespace DotNetTribes.Models
{
    public class Troop
    {
        public long TroopId { get; set; }
        public int KingdomId { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public long UpdatedAt { get; set; }
        public bool ConsumingFood { get; set; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Capacity { get; set; }

    }
}