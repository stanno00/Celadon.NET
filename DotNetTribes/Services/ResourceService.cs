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

        public ResourceService(ApplicationContext applicationContext, ITimeService timeService, IRulesService rulesService)
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

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
                var building = _applicationContext.Buildings.FirstOrDefault(b =>
                    b.KingdomId == kingdomId && b.Type == GetBuildingTypeByResourceType(resource.Type));

                if (building == null) continue;

                //resource.Generation = _rulesService.BuildingResourceGeneration(building);
                resource.Generation = resource.Type == ResourceType.Food
                    ? (_rulesService.BuildingResourceGeneration(building) - CalculateTroopConsumption(kingdomId))
                    : _rulesService.BuildingResourceGeneration(building);
                UpdateSingleResource(resource, kingdomId);
            }

            _applicationContext.SaveChanges();
        }

        public ResourcesDto GetKingdomResources(int kingdomId)
        {
            // Updating all resources before returning values to controller
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
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };

            return buildingType;
        }
        
        public bool ValidateTradeOffer(int id, TradeRequestDTO tradeRequestDto)
        {
            // UpdateKingdomResources(id);
            // var kingdom = _applicationContext.Kingdoms.FirstOrDefault(k => k.KingdomId == id);
            var hasMarketplace = _applicationContext.Buildings.Where(b => b.Type == BuildingType.Marketplace).FirstOrDefault(k => k.KingdomId == id);
            if (hasMarketplace == null)
            {
                throw new TargetException("You don't have a Marketplace");
            }

            var maxTradeAmount = _rulesService.MarketplaceTradeAmount(hasMarketplace.Level);
            if (maxTradeAmount < tradeRequestDto.offered_resource.amount || maxTradeAmount < tradeRequestDto.wanted_resource.amount)
            {
                throw new TargetException("Your marketplace isn't high enough level to trade this amount");
            }
            
            Resource resource = _applicationContext.Resources.Where(t => t.Type == tradeRequestDto.offered_resource.type)
                .FirstOrDefault(k => k.KingdomId == id);
            var resourcesAvailable = resource.Amount;

            if (tradeRequestDto.wanted_resource.type == tradeRequestDto.offered_resource.type)
            {
                throw new TargetException("You can't trade for the same resource");
            }
            if (resourcesAvailable < tradeRequestDto.offered_resource.amount)
            {
                throw new TargetException($"Not enough {resource.Type}");
            }

            resource.Amount -= tradeRequestDto.offered_resource.amount;

            var offer = new Offer
            {
                SellingType = tradeRequestDto.offered_resource.type,
                SellingAmount = tradeRequestDto.offered_resource.amount,
                SellerId = id,
                PayingType = tradeRequestDto.wanted_resource.type,
                PayingAmount = tradeRequestDto.wanted_resource.amount,
                Created_at = _timeService.GetCurrentSeconds(),
                Expire_at = _timeService.GetCurrentSeconds() + 86400
            };
            _applicationContext.Add(offer);
            _applicationContext.SaveChanges();

            return true;
        }

        public bool AcceptOffer(int id, int offerId)
        {
            var hasMarketplace = _applicationContext.Buildings.Where(b => b.Type == BuildingType.Marketplace).FirstOrDefault(k => k.KingdomId == id);
            if (hasMarketplace == null)
            {
                throw new TargetException("You don't have a Marketplace");
            }
            
            var offer = _applicationContext.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null || offer.Expire_at < _timeService.GetCurrentSeconds())
            {
                throw new TargetException("Offer doesn't exist");
            }

            if (offer.SellerId == id)
            {
                throw new TargetException("You can't accept your own trade offer");
            }

            var maxTradeAmount = _rulesService.MarketplaceTradeAmount(hasMarketplace.Level);
            if (maxTradeAmount < offer.SellingAmount || maxTradeAmount < offer.PayingAmount)
            {
                throw new TargetException("Your marketplace isn't high enough level to trade this amount");
            }
            
            var ResourceRequired = _applicationContext.Resources.Where(t => t.Type == offer.PayingType);
            var ResourceOffered = _applicationContext.Resources.Where(t => t.Type == offer.SellingType);

            if (ResourceRequired.FirstOrDefault(k => k.KingdomId == id).Amount < offer.PayingAmount)
            {
                throw new TargetException($"Not enough {offer.PayingType}");
            }
            
            Resource buyerResourceMinus = _applicationContext.Resources.Where(t => t.Type == offer.PayingType).FirstOrDefault(k => k.KingdomId == id);
            buyerResourceMinus.Amount -= offer.PayingAmount;
            Resource buyerResourcePlus = ResourceOffered.FirstOrDefault(k => k.KingdomId == id);
            buyerResourcePlus.Amount += offer.SellingAmount;
            
            Resource sellerResourcePlus = ResourceRequired.FirstOrDefault(k => k.KingdomId == offer.SellerId);
            sellerResourcePlus.Amount += offer.PayingAmount;

            offer.BuyerId = id;
            offer.Expire_at = _timeService.GetCurrentSeconds();

            _applicationContext.SaveChanges();
            
            return true;
        }

        public void UpdateOffers()
        {
            var offers = _applicationContext.Offers.Where(o => o.Expire_at < _timeService.GetCurrentSeconds() && o.BuyerId == null).ToList();
            foreach (var i in offers)
            {
                var id = _applicationContext.Users.FirstOrDefault(u => u.UserId == i.SellerId);

                Resource resource = _applicationContext.Resources.Where(r => r.Type == i.SellingType)
                    .FirstOrDefault(k => k.KingdomId == id.KingdomId);
                resource.Amount += i.SellingAmount;
            }

            _applicationContext.SaveChanges();
        }
    }
}