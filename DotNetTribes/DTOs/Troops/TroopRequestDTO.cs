using DotNetTribes.Enums;

namespace DotNetTribes.DTOs.Troops
{
    public class TroopRequestDTO
    {
        public int NumberOfTroops { get; set; }
        public BlackSmithTroops Name { get; set; }
    }
}