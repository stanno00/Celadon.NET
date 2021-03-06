using System;
using System.Linq;
using System.Reflection;
using DotNetTribes.DTOs;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.Enums;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ITimeService _timeService;
        private readonly IRulesService _rulesService;

        public ResourceService(ApplicationContext applicationContext, ITimeService timeService,
            IRulesService rulesService)
        {
            _applicationContext = applicationContext;
            _timeService = timeService;
            _rulesService = rulesService;
        }

        private void UpdateSingleResource(Resource resource, int kingdomId)
        {
            var minutesPassedSinceLastUpdate = (int) _timeService.MinutesSince(resource.UpdatedAt);

            // Unless a minute passes this method won't run
            if (minutesPassedSinceLastUpdate <= 0) return;


            //In the case that we have an army larger than we can feed, we try to do so before adding the new food to the kingdom's stores,
            //so that consumption is adjusted for how much food we get in this tick. This intends to prevent a scenario where we reach negative food.
            if (resource.Type == ResourceType.Food && resource.Amount < CalculateTroopConsumption(kingdomId))
            {
                FeedTroopsWithInsufficientFood(kingdomId);
            }

            resource.Amount += minutesPassedSinceLastUpdate * resource.Generation;
            resource.UpdatedAt = _timeService.GetCurrentSeconds();
        }

        public void UpdateKingdomResources(int kingdomId)
        {
            // Getting all resource generating buildings from the kingdom 
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();
            
            var wonBattles = _applicationContext.Battles
                .Where(b => b.ResourcesDeliveredToWinner == false 
                            && b.ReturnAt < _timeService.GetCurrentSeconds()
                            && b.WinnerId == kingdomId);
            
            foreach (var battle in wonBattles)
            {
                var food = kingdomResources.Single(r => r.Type == ResourceType.Food);
                food.Amount += battle.FoodStolen;

                var gold = kingdomResources.Single(r => r.Type == ResourceType.Gold);
                gold.Amount += battle.GoldStolen;

                battle.ResourcesDeliveredToWinner = true;
            }

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
                var building = _applicationContext.Buildings.FirstOrDefault(b =>
                    b.KingdomId == kingdomId && b.Type == GetBuildingTypeByResourceType(resource.Type));

                if (building == null) continue;

                //resource.Generation = _rulesService.BuildingResourceGeneration(building);
                resource.Generation = resource.Type == ResourceType.Food
                    ? (_rulesService.BuildingResourceGeneration(building, kingdomId) - CalculateTroopConsumption(kingdomId))
                    : _rulesService.BuildingResourceGeneration(building, kingdomId);
                UpdateSingleResource(resource, kingdomId);
            }

            _applicationContext.SaveChanges();
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            UpdateKingdomResources(kingdomId);
            
            var kingdomResourceDtoList = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .Select(r => new ResourceDto
                {
                    Amount = r.Amount,
                    Type = r.Type.ToString(),
                    UpdatedAt = r.UpdatedAt,
                    Generation = r.Generation
                })
                .ToList();

            var resources = new ResourcesDto
            {
                Resources = kingdomResourceDtoList
            };

            return resources;
        }

        private int CalculateTroopConsumption(int kingdomId)
        {
            var kingdomTroops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId && t.ConsumingFood)
                .ToList();
            int consumption = 0;
            foreach (var troop in kingdomTroops)
            {
                if (troop.ConsumingFood)
                {
                    consumption += _rulesService.TroopFoodConsumption(troop.Level);
                }
            }

            return consumption;
        }

        private void FeedTroopsWithInsufficientFood(int kingdomId)
        {
            var kingdomFood = _applicationContext.Resources
                .Single(r => r.Type == ResourceType.Food && r.KingdomId == kingdomId);
            var kingdomTroops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId && t.ConsumingFood)
                .OrderByDescending(t => t.Level)
                .ToList();
            foreach (var troop in kingdomTroops)
            {
                int foodNeeded = _rulesService.TroopFoodConsumption(troop.Level);
                if (kingdomFood.Amount > foodNeeded)
                {
                    kingdomFood.Amount -= foodNeeded;
                    troop.UpdatedAt = _timeService.GetCurrentSeconds();
                }
                else
                {
                    kingdomFood.Generation += _rulesService.TroopFoodConsumption(troop.Level);
                    _applicationContext.Troops.Remove(troop);
                }
            }

            _applicationContext.SaveChanges();
        }

        private BuildingType GetBuildingTypeByResourceType(ResourceType resourceType)
        {
            var buildingType = resourceType switch
            {
                ResourceType.Food => BuildingType.Farm,
                ResourceType.Gold => BuildingType.Mine,
                ResourceType.Iron => BuildingType.IronMine,
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };

            return buildingType;
        }

        public Offer ValidateCreateTradeOffer(int sellerKingdomId, TradeRequestDTO tradeRequestDto)
        {
            var hasMarketplace = _applicationContext.Buildings.Where(b => b.Type == BuildingType.Marketplace)
                .FirstOrDefault(k => k.KingdomId == sellerKingdomId);
            if (hasMarketplace == null)
            {
                throw new TargetException("You don't have a Marketplace");
            }

            var maxTradeAmount = _rulesService.MarketplaceTradeAmount(hasMarketplace.Level);
            if (maxTradeAmount < tradeRequestDto.OfferedResource.Amount ||
                maxTradeAmount < tradeRequestDto.WantedResource.Amount)
            {
                throw new TargetException("Your marketplace isn't high enough level to trade this amount");
            }

            Resource resource = _applicationContext.Resources.Where(t => t.Type == tradeRequestDto.OfferedResource.Type)
                .FirstOrDefault(k => k.KingdomId == sellerKingdomId);
            var resourcesAvailable = resource.Amount;

            if (tradeRequestDto.WantedResource.Type == tradeRequestDto.OfferedResource.Type)
            {
                throw new TargetException("You can't trade for the same resource");
            }

            if (resourcesAvailable < tradeRequestDto.OfferedResource.Amount)
            {
                throw new TargetException($"Not enough {resource.Type}");
            }

            resource.Amount -= tradeRequestDto.OfferedResource.Amount;

            var offer = new Offer
            {
                SellingType = tradeRequestDto.OfferedResource.Type,
                SellingAmount = tradeRequestDto.OfferedResource.Amount,
                SellerKingdomId = sellerKingdomId,
                PayingType = tradeRequestDto.WantedResource.Type,
                PayingAmount = tradeRequestDto.WantedResource.Amount,
                CreatedAt = _timeService.GetCurrentSeconds(),
                ExpireAt = _timeService.GetCurrentSeconds() + 60,
                ResourceReturned = false
            };
            _applicationContext.Add(offer);
            _applicationContext.SaveChanges();

            return offer;
        }

        public AcceptOfferResponseDTO AcceptOffer(int buyerKingdomId, int offerId)
        {
            var hasMarketplace = _applicationContext.Buildings.Where(b => b.Type == BuildingType.Marketplace)
                .FirstOrDefault(k => k.KingdomId == buyerKingdomId);
            if (hasMarketplace == null)
            {
                throw new TargetException("You don't have a Marketplace");
            }

            var offer = _applicationContext.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null || offer.ExpireAt < _timeService.GetCurrentSeconds())
            {
                throw new TargetException("Offer doesn't exist");
            }

            if (offer.SellerKingdomId == buyerKingdomId)
            {
                throw new TargetException("You can't accept your own trade offer");
            }

            var maxTradeAmount = _rulesService.MarketplaceTradeAmount(hasMarketplace.Level);
            if (maxTradeAmount < offer.SellingAmount || maxTradeAmount < offer.PayingAmount)
            {
                throw new TargetException("Your marketplace isn't high enough level to trade this amount");
            }

            var resourceRequired = _applicationContext.Resources.Where(t => t.Type == offer.PayingType)
                .FirstOrDefault(k => k.KingdomId == buyerKingdomId);
            var resourceOffered = _applicationContext.Resources.Where(t => t.Type == offer.SellingType)
                .FirstOrDefault(k => k.KingdomId == buyerKingdomId);

            if (resourceRequired.Amount < offer.PayingAmount)
            {
                throw new TargetException($"Not enough {offer.PayingType}");
            }

            Resource buyerResourceMinus = _applicationContext.Resources.Where(t => t.Type == offer.PayingType)
                .FirstOrDefault(k => k.KingdomId == buyerKingdomId);
            buyerResourceMinus.Amount -= offer.PayingAmount;
            Resource buyerResourcePlus = resourceOffered;
            buyerResourcePlus.Amount += offer.SellingAmount;

            Resource sellerResourcePlus = _applicationContext.Resources.Where(t => t.Type == offer.PayingType)
                .FirstOrDefault(k => k.KingdomId == offer.SellerKingdomId);
            sellerResourcePlus.Amount += offer.PayingAmount;

            offer.BuyerKingdomId = buyerKingdomId;
            offer.ExpireAt = _timeService.GetCurrentSeconds();

            _applicationContext.SaveChanges();

            return new AcceptOfferResponseDTO()
            {
                Status = "Accepted",
                Offer = offer
            };
        }

        public void UpdateOffers()
        {
            var offers = _applicationContext.Offers
                .Where(o => o.ExpireAt < _timeService.GetCurrentSeconds() && o.BuyerKingdomId == null &&
                            o.ResourceReturned == false).ToList();
            foreach (var i in offers)
            {
                var sellerKingdom =
                    _applicationContext.Kingdoms.FirstOrDefault(k => k.KingdomId == i.SellerKingdomId);

                Resource resource = _applicationContext.Resources.Where(r => r.Type == i.SellingType)
                    .FirstOrDefault(k => k.KingdomId == sellerKingdom.KingdomId);
                resource.Amount += i.SellingAmount;
                i.ResourceReturned = true;
            }

            _applicationContext.SaveChanges();
        }

        public int GetResourceAmount(int kingdomId, ResourceType resourceType)
        {
            var resourceAmount = _applicationContext.Resources.Single(r => r.KingdomId == kingdomId && r.Type == resourceType).Amount;
            return resourceAmount;
        }
    }
}