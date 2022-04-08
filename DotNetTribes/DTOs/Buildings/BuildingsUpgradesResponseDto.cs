namespace DotNetTribes.DTOs
{
    public class BuildingsUpgradesResponseDto
    {
        public string Name { get; set; }

        public long StartedAt { get; set; }
        
        public long FinishedAt { get; set; }
        public int KingdomId { get; set; }
    }
}