using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.DTOs.Trade;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class ResourceServiceTest
{
    [Fact]
    public void ResourceService_GetKingdomResources_returnsEmptyList()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ResourceDatabase")
            .Options;

        using var context = new ApplicationContext(options);
        context.Resources.Add(new Resource()
        {
            Amount = 80,
            Generation = 5,
            Type = ResourceType.Food,
            KingdomId = 1,
            UpdatedAt = 69857,
            ResourceId = 1
        });

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var resourceService = new ResourceService(context, timeServiceMock.Object, ruleServiceMock.Object);


        var resources = resourceService.GetKingdomResources(0);

        Assert.Empty(resources.Resources);
        Assert.NotNull(resources);
    }

    [Fact]
    public void ResourceService_GetKingdomResources_correctValues()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Resource Database")
            .Options;

        using var context = new ApplicationContext(options);
        context.Resources.Add(new Resource()
        {
            Amount = 80,
            Generation = 5,
            Type = ResourceType.Food,
            KingdomId = 1,
            UpdatedAt = 69857,
            ResourceId = 1
        });

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> rulesServiceMock = new Mock<IRulesService>();
        var resourceService = new ResourceService(context, timeServiceMock.Object, rulesServiceMock.Object);


        var resources = resourceService.GetKingdomResources(1);

        Assert.NotEmpty(resources.Resources);
        Assert.NotNull(resources);
        Assert.Equal(80, resources.Resources.ToArray()[0].Amount);
        Assert.Equal(1, resources.Resources.Count);
        Assert.Equal("Food", resources.Resources.ToArray()[0].Type);
    }

    [Fact]
    public void ResourceService_CreatingOfferWithOutMarketplace_ThrowsException()
    {
        var request = new TradeRequestDTO();
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer1")
            .Options;
        var context = new ApplicationContext(optionBuilder);

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.ValidateCreateTradeOffer(1, request));

        Assert.Equal("You don't have a Marketplace", exception.Message);
    }
    
    [Fact]
    public void ResourceService_OfferMoreThanMarketplaceLevel_ThrowsException()
    {
        var request = new TradeRequestDTO()
        {
            OfferedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Food,
                Amount = 5000
            },
            WantedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Gold,
                Amount = 5000
            }
        };
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer2")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 1,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 5000000,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 5000000,
                    KingdomId = 1
                }
            }
        };
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();

        rulesService.Setup(r => r.MarketplaceTradeAmount(1)).Returns(50);
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.ValidateCreateTradeOffer(1, request));

        Assert.Equal("Your marketplace isn't high enough level to trade this amount", exception.Message);
    }
    
    [Fact]
    public void ResourceService_TradingForTheSameRsource_ThrowsException()
    {
        var request = new TradeRequestDTO()
        {
            OfferedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Food,
                Amount = 0
            },
            WantedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Food,
                Amount = 0
            }
        };
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer3")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 5,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 5000000,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 5000000,
                    KingdomId = 1
                }
            }
        };
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.ValidateCreateTradeOffer(1, request));

        Assert.Equal("You can't trade for the same resource", exception.Message);
    }
    
    [Fact]
    public void ResourceService_OfferingMoreResourceThanAvailable_ThrowsException()
    {
        var request = new TradeRequestDTO()
        {
            OfferedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Food,
                Amount = 10
            },
            WantedResource = new TypeAmountDTO()
            {
                Type = ResourceType.Gold,
                Amount = 0
            }
        };
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer4")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 1,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 1
                }
            }
        };
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        rulesService.Setup(r => r.MarketplaceTradeAmount(1)).Returns(50);
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.ValidateCreateTradeOffer(1, request));

        Assert.Equal("Not enough Food", exception.Message);
    }
    
    [Fact]
    public void ResourceService_AcceptingOfferWithOutMarketplace_ThrowsException()
    {
        var request = new TradeRequestDTO();
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer5")
            .Options;
        var context = new ApplicationContext(optionBuilder);

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.AcceptOffer(1, 1));

        Assert.Equal("You don't have a Marketplace", exception.Message);
    }
    
    [Fact]
    public void ResourceService_AcceptingOfferThatExpired_ThrowsException()
    {
        var request = new TradeRequestDTO();
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer6")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 5,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 1
                }
            }
        };
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.AcceptOffer(1, 1));

        Assert.Equal("Offer doesn't exist", exception.Message);
    }
    
    [Fact]
    public void ResourceService_AcceptingYourOwnOffer_ThrowsException()
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer7")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var offer = new Offer()
        {
            OfferId = 1,
            SellerKingdomId = 1
        };
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 5,
                    KingdomId = 1
                }
            }
        };
        context.Add(kingdom);
        context.Add(offer);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.AcceptOffer(1, 1));

        Assert.Equal("You can't accept your own trade offer", exception.Message);
    }
    
    [Fact]
    public void ResourceService_AcceptingOfferWithMarketplaceTooLow_ThrowsException()
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer8")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 1,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 9999,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 9999,
                    KingdomId = 1
                }
            }
        };
        var offer = new Offer()
        {
            OfferId = 1,
            PayingType = ResourceType.Food,
            PayingAmount = 999,
            SellingType = ResourceType.Gold,
            SellingAmount = 999,
            SellerKingdomId = 2
        };
        context.Add(kingdom);
        context.Add(offer);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.AcceptOffer(1, 1));

        Assert.Equal("Your marketplace isn't high enough level to trade this amount", exception.Message);
    }
    
    [Fact]
    public void ResourceService_AcceptingOfferWithInsufficientResource_ThrowsException()
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer9")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 1,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 9999,
                    KingdomId = 1
                }
            }
        };
        var offer = new Offer()
        {
            OfferId = 1,
            PayingType = ResourceType.Food,
            PayingAmount = 10,
            SellingType = ResourceType.Gold,
            SellingAmount = 10,
            SellerKingdomId = 2
        };
        context.Add(kingdom);
        context.Add(offer);
        context.SaveChanges();

        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        rulesService.Setup(r => r.MarketplaceTradeAmount(1)).Returns(50);
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        var exception = Record.Exception(() => controller.AcceptOffer(1, 1));

        Assert.Equal("Not enough Food", exception.Message);
    }

    [Fact]
    public void ResourceService_OfferExpired_ReturnsResource()
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Offer10")
            .Options;
        var context = new ApplicationContext(optionBuilder);
        var kingdom = new Kingdom()
        {
            KingdomId = 1,
            Buildings = new List<Building>()
            {
                new Building()
                {
                    BuildingId = 1,
                    Type = BuildingType.Marketplace,
                    Level = 1,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource()
                {
                    ResourceId = 1,
                    Type = ResourceType.Food,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource()
                {
                    ResourceId = 2,
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 1
                }
            }
        };
        var offer = new Offer()
        {
            OfferId = 1,
            PayingType = ResourceType.Food,
            PayingAmount = 20,
            SellingType = ResourceType.Gold,
            SellingAmount = 10,
            SellerKingdomId = 1,
            ExpireAt = -1,
            ResourceReturned = false,
            BuyerKingdomId = null
        };
        context.Add(kingdom);
        context.Add(offer);
        context.SaveChanges();
        
        Mock<ITimeService> timeService = new Mock<ITimeService>();
        Mock<IRulesService> rulesService = new Mock<IRulesService>();
        timeService.Setup(t => t.GetCurrentSeconds()).Returns(0);
        var controller = new ResourceService(context, timeService.Object, rulesService.Object);
        controller.UpdateOffers();
        var resources = context.Resources.Where(r => r.KingdomId == 1);
        var updatedOffer = context.Offers.FirstOrDefault(o => o.OfferId == 1);
        
        Assert.Equal(10,resources.FirstOrDefault(r => r.Type == ResourceType.Gold).Amount);
        Assert.Equal(0,resources.FirstOrDefault(r => r.Type == ResourceType.Food).Amount);
        Assert.True(updatedOffer.ResourceReturned);
        
    }
    
        [Fact]
    public void FeedTroopsWithInsufResources_KillsCorrectTroop()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ResourceTestDB3")
            .Options;

        using var context = new ApplicationContext(options);

        context.Kingdoms.Add(new Kingdom
        {
            KingdomId = 1,
            Resources = new List<Resource>
            {
                new Resource
                {
                    KingdomId = 1,
                    Amount = 4,
                    Type = ResourceType.Food,
                    UpdatedAt = 0
                }
            },
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Level = 2,
                    ConsumingFood = true
                },
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 2,
                    Level = 1,
                    ConsumingFood = true
                }
            },
            Buildings = new List<Building>
            {
                new Building
                {
                    KingdomId = 1,
                    BuildingId = 1,
                    Type = BuildingType.Farm,
                    Level = 1,
                    Finished_at = 0
                }
            }
        });
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(60);
        timeServiceMock.Setup(x => x.MinutesSince(0)).Returns(1);

        Mock<IRulesService> rulesServiceMock = new Mock<IRulesService>();
        rulesServiceMock.Setup(x => x.TroopFoodConsumption(1)).Returns(2);
        rulesServiceMock.Setup(x => x.TroopFoodConsumption(2)).Returns(4);
        
        var resourceService = new ResourceService(context, timeServiceMock.Object, rulesServiceMock.Object);

        resourceService.UpdateKingdomResources(1);

        Assert.Single(context.Troops.Where(t => t.KingdomId == 1).ToList());
        Assert.Equal(2, context.Troops.FirstOrDefault(t => t.KingdomId == 1).Level);
    }
}