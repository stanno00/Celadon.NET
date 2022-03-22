namespace DotNetTribes.DTOs
{
    public class BuildingDTO
    {
        public long BuildingId { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public int KingdomId { get; set; }
    }
}