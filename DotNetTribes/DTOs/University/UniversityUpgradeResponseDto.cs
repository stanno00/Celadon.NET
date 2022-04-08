using DotNetTribes.Enums;

namespace DotNetTribes.DTOs.University
{
    public class UniversityUpgradeResponseDto
    {
        public string Status { get; set; }
        public UpgradeType UpgradeType { get; set; }
        public int Level { get; set; }
    }
}