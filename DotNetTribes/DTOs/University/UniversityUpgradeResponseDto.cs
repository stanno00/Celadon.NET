using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.Enums;

namespace DotNetTribes.DTOs.University
{
    public class UniversityUpgradeResponseDto
    {
        public string Status { get; set; }
        public string UpgradeType { get; set; }
        public int CurrentLevel { get; set; }
    }
}