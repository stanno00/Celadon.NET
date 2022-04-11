namespace DotNetTribes.Models
{
    public class Troop
    {
        public long TroopId { get; set; }
        public int KingdomId { get; set; }
        public long StartedAt { get; set; }
        public int TroopHP { get; set; }
        public long FinishedAt { get; set; }
        public long UpdatedAt { get; set; }
        public bool ConsumingFood { get; set; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Capacity { get; set; }
        public int BattleId { get; set; }
        public long ReturnedFromBattleAt { get; set; }
        public string? Name { get; set; }
    }
}