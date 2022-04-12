using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class UniversityUpgrade
    {
        public long UniversityUpgradeId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public UpgradeType UpgradeType { get; set; }
        public double EffectStrength { get; set; }
        public int Level { get; set; }
        public int KingdomId { get; set; }
        public long StartedAt { get; set; }
        public long FinishedAt { get; set; }
        public bool AddedToKingdom { get; set; }
        
    }
}