using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType TypeOffered { get; set; }
        public int AmountOffered { get; set; }
        public int UserOfferId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType TypeRequired { get; set; }
        public int AmountRequired { get; set; }
        public int? UserAcceptedId { get; set; }
        public long Started_at { get; set; }
        public long Finished_at { get; set; }
    }
}