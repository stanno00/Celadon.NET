using System.Collections;
using System.Collections.Generic;
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
        var request = new TroopRequestDTO
        {
            NumberOfTroops = 1
        };

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
        var request = new TroopRequestDTO
        {
            NumberOfTroops = 1
        };

        //Simulating 0 capacity by not mocking a "real response" to avoid having to set up 100 soliders
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
        var request = new TroopRequestDTO
        {
            NumberOfTroops = 1
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
         
         Assert.Equal(0, result.Troops.Count);
         
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
}