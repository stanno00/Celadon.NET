namespace DotNetTribes.DTOs
{
    public class BuildingResponseDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public string Started_at { get; set; }
        public string Finished_at { get; set; }
        
    }
}