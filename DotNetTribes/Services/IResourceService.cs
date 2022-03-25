using System.Collections.Generic;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;

namespace DotNetTribes.Services
{
    public interface IResourceService
    {
        ResourcesDto GetKingdomResources(int kingdomId);
        void UpdateKingdomResources(int kingdomId);
        
        bool ValidateTradeOffer(int id, TradeRequestDTO tradeRequestDto);
        bool AcceptOffer(int id, int offerId);
        void UpdateOffers();
    }
}