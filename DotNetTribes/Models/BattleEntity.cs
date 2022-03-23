namespace DotNetTribes.Models
{
    public class BattleEntity
    {
        public int BattleEntityId { get; set; }
        public int AttackerId { get; set; }
        public int DefenderId { get; set; }
        public long FightStart { get; set; }
        public long ArriveAt { get; set; }
        public long ReturnAt { get; set; }
        public int WinnerId { get; set; }
        public int FoodStolen { get; set; }
        public int GoldStolen { get; set; }
        public int LostTroopsDefender { get; set; }
        public int LostTroopsAttacker { get; set; }
    }
}