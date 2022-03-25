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

        private void UpdateSingleResource(Resource resource)
        {
            var minutesPassedSinceLastUpdate = (int)_timeService.MinutesSince(resource.UpdatedAt);

            // Unless a minute passes this method wont run
            if (minutesPassedSinceLastUpdate <= 0) return;

            resource.Amount += minutesPassedSinceLastUpdate * resource.Generation;
            resource.UpdatedAt = _timeService.GetCurrentSeconds();

        }

        public void UpdateKingdomResources(int kingdomId)
        {
            // Getting all resource generation buildings from kingdom 
            var kingdomResources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId)
                .ToList();

            // Updating each resource
            foreach (var resource in kingdomResources)
            {
                var building = _applicationContext.Buildings.FirstOrDefault(b =>
                    b.KingdomId == kingdomId && b.Type == GetBuildingTypeByResourceType(resource.Type));
                
                if (building == null) continue;
                
                resource.Generation = _rulesService.BuildingResourceGeneration(building);
                UpdateSingleResource(resource);
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
                    UpdatedAt = r.UpdatedAt
                })
                .ToList();
            
            var resources = new ResourcesDto
            {
                Resources = kingdomResourceDtoList
            };

            return resources;
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
            UpdateKingdomResources(id);
            var 
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
                TypeOffered = tradeRequestDto.offered_resource.type,
                AmountOffered = tradeRequestDto.offered_resource.amount,
                UserOfferId = id,
                TypeRequired = tradeRequestDto.wanted_resource.type,
                AmountRequired = tradeRequestDto.wanted_resource.amount,
                Started_at = _timeService.GetCurrentSeconds(),
                Finished_at = _timeService.GetCurrentSeconds() + DateTimeOffset.Now.AddHours(24).ToUnixTimeSeconds()
            };
            _applicationContext.Add(offer);
            _applicationContext.SaveChanges();

            return true;
        }

        public bool AcceptOffer(int id, int offerId)
        {
            var offer = _applicationContext.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null || offer.Finished_at < _timeService.GetCurrentSeconds())
            {
                throw new TargetException("Offer doesn't exist");
            }
            
            var ResourceRequired = _applicationContext.Resources.Where(t => t.Type == offer.TypeRequired);
            var ResourceOffered = _applicationContext.Resources.Where(t => t.Type == offer.TypeOffered);

            var kingdom = _applicationContext.Kingdoms.FirstOrDefault(k => k.KingdomId == id);
            if (ResourceRequired.FirstOrDefault(k => k.KingdomId == id).Amount < offer.AmountRequired)
            {
                throw new TargetException($"Not enough {offer.TypeRequired}");
            }
            
            Resource buyerResourceMinus = _applicationContext.Resources.Where(t => t.Type == offer.TypeRequired).FirstOrDefault(k => k.KingdomId == id);
            buyerResourceMinus.Amount -= offer.AmountRequired;
            Resource buyerResourcePlus = ResourceOffered.FirstOrDefault(k => k.KingdomId == id);
            buyerResourcePlus.Amount += offer.AmountOffered;
            
            Resource sellerResourcePlus = ResourceRequired.FirstOrDefault(k => k.KingdomId == offer.UserOfferId);
            sellerResourcePlus.Amount += offer.AmountRequired;

            offer.UserAcceptedId = id;
            offer.Finished_at = _timeService.GetCurrentSeconds();

            _applicationContext.SaveChanges();
            
            return true;
        }
    }
}