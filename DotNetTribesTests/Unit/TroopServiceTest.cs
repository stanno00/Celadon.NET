using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class TroopServiceTest
{
    [Fact]
    public void TrainTroops_WithoutAcademy_ThrowsException()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    BuildingId = 1,
                    KingdomId = 1,
                    Type = BuildingType.Farm
                },
                new Building
                {
                    BuildingId = 2,
                    KingdomId = 1,
                    Type = BuildingType.Mine
                }
            },
            Troops = new List<Troop>(),
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    KingdomId = 1,
                    Amount = 500
                }
            }
        };

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(1)).Returns(25);
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest1")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.TrainTroops(1, new TroopRequestDTO
        {
            NumberOfTroops = 1
        }));

        Assert.Equal("You need an academy to be able to train troops.", exception.Message);
    }

    [Fact]
    public void TrainTroops_WithInsufficientCapacity_ThrowsException()
    {
        //Simulating 0 capacity by not mocking a "real response" to avoid having to set up 100 soldiers
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    BuildingId = 1,
                    KingdomId = 1,
                    Type = BuildingType.Farm
                },
                new Building
                {
                    BuildingId = 2,
                    KingdomId = 1,
                    Type = BuildingType.Mine
                },
                new Building
                {
                    BuildingId = 3,
                    KingdomId = 1,
                    Type = BuildingType.Academy
                },
                new Building
                {
                    BuildingId = 4,
                    KingdomId = 1,
                    Type = BuildingType.TownHall,
                    Level = 1
                }
            },
            Troops = new List<Troop>(),
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    KingdomId = 1,
                    Amount = 500
                }
            }
        };

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(1)).Returns(25);
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest2")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.TrainTroops(1, new TroopRequestDTO
        {
            NumberOfTroops = 1
        }));

        Assert.Equal("Insufficient troop capacity.", exception.Message);
    }

    [Fact]
    public void TrainTroops_WithInsufficientGold_ThrowsException()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    BuildingId = 4,
                    Type = BuildingType.TownHall,
                    KingdomId = 1,
                    Level = 1
                },
                new Building
                {
                    BuildingId = 1,
                    KingdomId = 1,
                    Type = BuildingType.Farm
                },
                new Building
                {
                    BuildingId = 2,
                    KingdomId = 1,
                    Type = BuildingType.Mine
                },
                new Building
                {
                    BuildingId = 3,
                    KingdomId = 1,
                    Type = BuildingType.Academy
                }
            },
            Troops = new List<Troop>(),
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    KingdomId = 1,
                    Amount = 10
                }
            }
        };

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(1)).Returns(25);
        ruleServiceMock.Setup(x => x.StorageLimit(1)).Returns(100);
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest3")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.TrainTroops(1, new TroopRequestDTO
        {
            NumberOfTroops = 1
        }));

        Assert.Equal("Not enough gold.", exception.Message);
    }

    [Fact]
    public void TrainTroops_WithCorrectInputs_ReturnsCorrectTroops()
    {
        var request = new TroopRequestDTO
        {
            NumberOfTroops = 3
        };

        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Buildings = new List<Building>
            {
                new Building
                {
                    BuildingId = 4,
                    Type = BuildingType.TownHall,
                    KingdomId = 1,
                    Level = 1
                },
                new Building
                {
                    BuildingId = 3,
                    KingdomId = 1,
                    Type = BuildingType.Academy
                }
            },
            Troops = new List<Troop>(),
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    KingdomId = 1,
                    Amount = 500
                }
            }
        };

        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(1)).Returns(25);
        ruleServiceMock.Setup(x => x.StorageLimit(1)).Returns(100);
        ruleServiceMock.Setup(x => x.TroopAttack(1)).Returns(10);
        ruleServiceMock.Setup(x => x.TroopDefense(1)).Returns(5);
        ruleServiceMock.Setup(x => x.TroopCapacity(1)).Returns(2);
        ruleServiceMock.Setup(x => x.TroopBuildingTime(1)).Returns(30);
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest5")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        var result = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object).TrainTroops(1, request);

        var expectedSoldier1 = new Troop
        {
            StartedAt = 0,
            FinishedAt = 30,
            KingdomId = 1,
            Attack = 10,
            Defense = 5,
            Capacity = 2,
            ConsumingFood = false
        };
        var expectedSoldier2 = new Troop
        {
            StartedAt = 30,
            FinishedAt = 60,
            KingdomId = 1,
            Attack = 10,
            Defense = 5,
            Capacity = 2
        };
        var expectedSoldier3 = new Troop
        {
            StartedAt = 60,
            FinishedAt = 90,
            KingdomId = 1,
            Attack = 10,
            Defense = 5,
            Capacity = 2
        };

        Assert.Equal(expectedSoldier1.StartedAt, result.NewTroops[0].StartedAt);
        Assert.Equal(expectedSoldier2.StartedAt, result.NewTroops[1].StartedAt);
        Assert.Equal(expectedSoldier3.StartedAt, result.NewTroops[2].StartedAt);
        Assert.Equal(expectedSoldier1.FinishedAt, result.NewTroops[0].FinishedAt);
        Assert.Equal(expectedSoldier2.FinishedAt, result.NewTroops[1].FinishedAt);
        Assert.Equal(expectedSoldier3.FinishedAt, result.NewTroops[2].FinishedAt);
        Assert.Equal(expectedSoldier1.Attack, result.NewTroops[0].Attack);
        Assert.Equal(expectedSoldier1.Defense, result.NewTroops[0].Defense);
        Assert.Equal(expectedSoldier1.Capacity, result.NewTroops[0].Capacity);
        Assert.Equal(expectedSoldier1.ConsumingFood, result.NewTroops[0].ConsumingFood);
        Assert.Equal(expectedSoldier1.KingdomId, result.NewTroops[0].KingdomId);
    }

    [Fact]
    public void GetKingdomTroops_WithZeroTroops_ReturnsEmptyList()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>()
        };

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest6")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        var result = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object).GetKingdomTroops(1);

        Assert.Empty(result.Troops);
    }

    [Fact]
    public void GetKingdomTroops_WithOneTroop_ReturnsCorrectResult()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = true,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 60
                }
            }
        };

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest7")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        var result = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object).GetKingdomTroops(1);

        Assert.Single(result.Troops);
        Assert.Equal(1, result.Troops[0].KingdomId);
        Assert.Equal(1, result.Troops[0].TroopId);
        Assert.Equal(10, result.Troops[0].Attack);
        Assert.Equal(5, result.Troops[0].Defense);
        Assert.Equal(2, result.Troops[0].Capacity);
        Assert.True(result.Troops[0].ConsumingFood);
        Assert.Equal(0, result.Troops[0].StartedAt);
        Assert.Equal(30, result.Troops[0].FinishedAt);
        Assert.Equal(60, result.Troops[0].UpdatedAt);
        Assert.Equal(1, result.Troops[0].Level);
    }

    [Fact]
    public void UpgradeTroops_WithIncorrectTroopID_ThrowsException()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = true,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 60
                }
            }
        };

        var foreignTroop = new Troop
        {
            TroopId = 2,
            KingdomId = 2
        };

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest8")
            .Options;

        var context = new ApplicationContext(optionsBuilder);

        context.Troops.Add(foreignTroop);
        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.UpgradeTroops(1, new TroopUpgradeRequestDTO
        {
            TroopIds = new List<long>
            {
                2
            }
        }));

        Assert.Equal("Invalid Troop ID.", exception.Message);
    }

    [Fact]
    public void UpgradeTroops_WithLowLevelAcademy_ThrowsException()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = true,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 60
                }
            },
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.Academy,
                    Level = 1,
                    KingdomId = 1
                }
            }
        };


        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest9")
            .Options;

        var context = new ApplicationContext(optionsBuilder);


        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.UpgradeTroops(1, new TroopUpgradeRequestDTO
        {
            TroopIds = new List<long>
            {
                1
            }
        }));

        Assert.Equal("Upgrade not allowed, academy level too low.", exception.Message);
    }

    [Fact]
    public void UpgradeTroops_WithInsufficientGold_ThrowsException()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = true,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 60
                }
            },
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.Academy,
                    Level = 2,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 5,
                    KingdomId = 1
                }
            }
        };


        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest10")
            .Options;

        var context = new ApplicationContext(optionsBuilder);


        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(2)).Returns(50);

        var controller = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        var exception = Record.Exception(() => controller.UpgradeTroops(1, new TroopUpgradeRequestDTO
        {
            TroopIds = new List<long>
            {
                1
            }
        }));

        Assert.Equal("Not enough gold to upgrade.", exception.Message);
    }

    [Fact]
    public void UpgradeTroops_WithCorrectInputs_ReturnsCorrectOutput()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = true,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 60
                }
            },
            Buildings = new List<Building>
            {
                new Building
                {
                    Type = BuildingType.Academy,
                    Level = 2,
                    KingdomId = 1
                }
            },
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Gold,
                    Amount = 500,
                    KingdomId = 1
                }
            }
        };


        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest11")
            .Options;

        var context = new ApplicationContext(optionsBuilder);


        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        ruleServiceMock.Setup(x => x.TroopPrice(2)).Returns(50);
        ruleServiceMock.Setup(x => x.TroopAttack(2)).Returns(20);
        ruleServiceMock.Setup(x => x.TroopDefense(2)).Returns(10);
        ruleServiceMock.Setup(x => x.TroopCapacity(2)).Returns(4);
        ruleServiceMock.Setup(x => x.TroopBuildingTime(2)).Returns(60);
        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

        var result = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object).UpgradeTroops(1, new TroopUpgradeRequestDTO
        {
            TroopIds = new List<long>
            {
                1
            }
        });

        Assert.Equal(1, result.Troops[0].TroopId);
        Assert.Equal(1, result.Troops[0].KingdomId);
        Assert.Equal(20, result.Troops[0].Attack);
        Assert.Equal(10, result.Troops[0].Defense);
        Assert.Equal(4, result.Troops[0].Capacity);
        Assert.False(result.Troops[0].ConsumingFood);
        Assert.Equal(30, result.Troops[0].StartedAt);
        Assert.Equal(90, result.Troops[0].FinishedAt);
    }

    [Fact]
    public void UpdateTroops_WithOneSoldierTrained_ChangesStatus()
    {
        var kingdom = new Kingdom
        {
            KingdomId = 1,
            Troops = new List<Troop>
            {
                new Troop
                {
                    KingdomId = 1,
                    TroopId = 1,
                    Attack = 10,
                    Defense = 5,
                    Capacity = 2,
                    ConsumingFood = false,
                    StartedAt = 0,
                    FinishedAt = 30,
                    Level = 1,
                    UpdatedAt = 0
                }
            },
            Buildings = new List<Building>
            {
                new Building
                {
                   Type = BuildingType.Farm,
                   BuildingId = 1,
                   KingdomId = 1,
                   Level = 1
                }
            },
            Resources = new List<Resource>
            {
                new Resource
                {
                    Type = ResourceType.Food,
                    Amount = 0,
                    UpdatedAt = 0,
                    Generation = 0,
                    KingdomId = 1,
                    ResourceId = 1
                }
            }
        };


        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TroopTest12")
            .Options;

        var context = new ApplicationContext(optionsBuilder);


        context.Kingdoms.Add(kingdom);
        context.SaveChanges();

        Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
        Mock<IResourceService> resourceServiceMock = new Mock<IResourceService>();

        timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(60);

        var ts = new TroopService(context, ruleServiceMock.Object, timeServiceMock.Object, resourceServiceMock.Object);
        ts.UpdateTroops(1);
        
        Assert.True(context.Troops.Single(t => t.KingdomId == 1).ConsumingFood);
    }
}