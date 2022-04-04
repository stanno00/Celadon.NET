using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType SellingType { get; set; }
        public int SellingAmount { get; set; }
        public int SellerKingdomId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType PayingType { get; set; }
        public int PayingAmount { get; set; }
        public int? BuyerKingdomId { get; set; }
        public long CreatedAt { get; set; }
        public long ExpireAt { get; set; }
        public bool ResourceReturned { get; set; }
    }
}