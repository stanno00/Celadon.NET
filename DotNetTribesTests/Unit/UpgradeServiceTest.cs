using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class UpgradeServiceTest
{
    [Fact]
    public void UniversityUpgradeService_BuyBuildingBuildSpeed_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade1")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.BuildingBuildSpeed);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.BuildingBuildSpeed, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    [Fact]
    public void UniversityUpgradeService_UpgradeBuildingBuildSpeed_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade2")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 0.05,
                    UpgradeType = UpgradeType.BuildingBuildSpeed,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.BuildingBuildSpeed);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.BuildingBuildSpeed, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_BuyTroopsTrainSpeed_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade3")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.TroopsTrainSpeed);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.TroopsTrainSpeed, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_UpgradeTroopsTrainSpeed_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade4")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 0.05,
                    UpgradeType = UpgradeType.TroopsTrainSpeed,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.TroopsTrainSpeed);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.TroopsTrainSpeed, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_BuyFarmProduceBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade5")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.FarmProduceBonus);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.FarmProduceBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_UpgradeFarmProduceBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade6")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 0.05,
                    UpgradeType = UpgradeType.FarmProduceBonus,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.FarmProduceBonus);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.FarmProduceBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_BuyMineProduceBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade7")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.MineProduceBonus);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.MineProduceBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_UpgradeMineProduceBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade8")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 0.05,
                    UpgradeType = UpgradeType.MineProduceBonus,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            },
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.MineProduceBonus);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(0.05, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.MineProduceBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_BuyAllTroopsAtkBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade9")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>(),
            Troops = new List<Troop>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsAtkBonus);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(3, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.AllTroopsAtkBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_UpgradeAllTroopsAtkBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade10")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 3,
                    UpgradeType = UpgradeType.AllTroopsAtkBonus,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            },
            Troops = new List<Troop>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsAtkBonus);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(3, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.AllTroopsAtkBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_BuyAllTroopsDefBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade11")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsDefBonus);
        
        Assert.Equal(0, buildingSpeed.Level);
        Assert.Equal(2, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.AllTroopsDefBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_UpgradeAllTroopsDefBonus_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade12")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.AllTroopsDefBonus,
                    KingdomId = 1,
                    StartedAt = -11,
                    FinishedAt = -1,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsDefBonus);
        
        Assert.Equal(1, buildingSpeed.Level);
        Assert.Equal(2, buildingSpeed.EffectStrength);
        Assert.Equal(1, buildingSpeed.KingdomId);
        Assert.Equal(1, buildingSpeed.UniversityUpgradeId);
        Assert.Equal(UpgradeType.AllTroopsDefBonus, buildingSpeed.UpgradeType);
        Assert.False(buildingSpeed.AddedToKingdom);
    }
    
    [Fact]
    public void UniversityUpgradeService_GetUniversityLevel2TimeReduction_ReturnUniversityUpgrade()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade13")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 2
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.AllTroopsDefBonus,
                    KingdomId = 1,
                    StartedAt = 0,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);

        var buildingSpeed = upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsDefBonus);
        
        Assert.Equal(-2, buildingSpeed.FinishedAt);
    }

    [Fact]
    public void UpgradeService_ByuUniversityUpgradeWithoutUniversity_ThrowsException()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade14")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>(),
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);
        var exception = Record.Exception(() => upgradeService.BuyUniversityUpgrade(1, UpgradeType.BuildingBuildSpeed));

        Assert.Equal("You don't have a University", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_BuyUniversityUpgradeWhileAnotherUpgradeIsInProgress_ThrowsException()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade15")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 1,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.AllTroopsDefBonus,
                    KingdomId = 1,
                    FinishedAt = 300,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);
        var exception = Record.Exception(() => upgradeService.BuyUniversityUpgrade(1, UpgradeType.BuildingBuildSpeed));

        Assert.Equal("There is an upgrade in progress", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_BuyUniversityUpgradeOverMaxLevel_ThrowsException()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade16")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 5,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.AllTroopsDefBonus,
                    KingdomId = 1,
                    FinishedAt = 0,
                    AddedToKingdom = true
                }
            }
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);
        var exception = Record.Exception(() => upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsDefBonus));

        Assert.Equal("You have max level of AllTroopsDefBonus", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_BuyUniversityWithInsufficientGold_ThrowsException()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade17")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
        };
        
        context.Add(kingdom);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);
        var exception = Record.Exception(() => upgradeService.BuyUniversityUpgrade(1, UpgradeType.AllTroopsDefBonus));

        Assert.Equal("Not enough resources", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_ApplyUpgradesWhenFinished_UpdatesUpgrades()
    {
        var option = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("Upgrade18")
            .Options;
        using var context = new ApplicationContext(option);

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 1,
                    KingdomId = 1,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 1
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 1
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 1,
                    Level = 0,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.AllTroopsDefBonus,
                    KingdomId = 1,
                    StartedAt = -11,
                    FinishedAt = -1,
                    AddedToKingdom = false
                }
            },
            Troops = new List<Troop>()
            {
                new Troop()
                {
                    TroopId = 1,
                    Attack = 4,
                    Capacity = 1,
                    Defense = 0,
                    Level = 1,
                    ConsumingFood = false,
                    KingdomId = 1,
                    StartedAt = -11,
                    FinishedAt = -1,
                    UpdatedAt = -2
                },
                new Troop()
                {
                    TroopId = 2,
                    Attack = 4,
                    Capacity = 1,
                    Defense = 1,
                    Level = 1,
                    ConsumingFood = false,
                    KingdomId = 1,
                    StartedAt = -2,
                    FinishedAt = 300,
                    UpdatedAt = -2
                }
            }
        };
        
        var kingdom2 = new Kingdom
        {
            KingdomId = 2,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 2,
                    KingdomId = 2,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 0,
                    KingdomId = 2
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 2
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 2,
                    Level = 1,
                    EffectStrength = 2,
                    UpgradeType = UpgradeType.MineProduceBonus,
                    KingdomId = 2,
                    StartedAt = -11,
                    FinishedAt = -3,
                    AddedToKingdom = true
                }
            }
        };
        
        var kingdom3 = new Kingdom
        {
            KingdomId = 3,
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.University,
                    BuildingId = 3,
                    KingdomId = 3,
                    Level = 1
                }
            },
            Resources = new List<Resource>()
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 2
                },
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 500,
                    KingdomId = 2
                }
            },
            Upgrades = new List<UniversityUpgrade>()
            {
                new UniversityUpgrade()
                {
                    UniversityUpgradeId = 3,
                    Level = 0,
                    EffectStrength = 3,
                    UpgradeType = UpgradeType.AllTroopsAtkBonus,
                    FinishedAt = -1,
                    AddedToKingdom = false
                },
            },
            Troops = new List<Troop>()
            {
                new Troop()
                {
                    TroopId = 3,
                    Attack = 1
                }
            }
        };
        
        
        context.Add(kingdom);
        context.Add(kingdom2);
        context.Add(kingdom3);
        context.SaveChanges();

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        var upgradeService = new UpgradeService(context, ruleServiceMock.Object, timeServiceMock.Object);
        upgradeService.ApplyUpgradesWhenFinished(1);
        upgradeService.ApplyUpgradesWhenFinished(2);
        upgradeService.ApplyUpgradesWhenFinished(3);

        var upgrade1 = context.UniversityUpgrades.FirstOrDefault(u => u.UniversityUpgradeId == 1);
        var upgrade2 = context.UniversityUpgrades.FirstOrDefault(u => u.UniversityUpgradeId == 2);
        var upgrade3 = context.UniversityUpgrades.FirstOrDefault(u => u.UniversityUpgradeId == 3);
        var troop3 = context.Troops.FirstOrDefault(t => t.TroopId == 3);

        Assert.Equal(1, upgrade1.Level);
        Assert.True(upgrade1.AddedToKingdom);
        Assert.Equal(1, upgrade2.Level);
        Assert.Equal(1, upgrade3.Level);
        Assert.True(upgrade3.AddedToKingdom);
        Assert.Equal(4, troop3.Attack);
    }
}