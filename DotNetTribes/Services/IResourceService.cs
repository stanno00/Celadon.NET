using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;

namespace DotNetTribes.Services
{
    public interface IResourceService
    {
        ResourcesDto GetKingdomResources(int kingdomId);
        void UpdateKingdomResources(int kingdomId);
        
        bool ValidateCreateTradeOffer(int id, TradeRequestDTO tradeRequestDto);
        AcceptOfferResponseDTO AcceptOffer(int id, int offerId);
        void UpdateOffers();
    }
}