using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType SellingType { get; set; }
        public int SellingAmount { get; set; }
        public int SellerId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType PayingType { get; set; }
        public int PayingAmount { get; set; }
        public int? BuyerId { get; set; }
        public long Created_at { get; set; }
        public long Expire_at { get; set; }
    }
}