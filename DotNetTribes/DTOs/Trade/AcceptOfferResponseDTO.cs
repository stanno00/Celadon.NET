using DotNetTribes.Models;

namespace DotNetTribes.DTOs.Trade
{
    public class AcceptOfferResponseDTO
    {
        public string Status { get; set; }
        public Offer Offer { get; set; }
    }
}