using System.Collections.Generic;
using DotNetTribes;
using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit
{

    public class BuildingServiceTest
    {
        [Fact]
        public void CreateNewBuilding_WithEmptyInputString_ThrowsException()
        {
            var request = new BuildingRequestDTO
            {
                Type = ""
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest1")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.CreateNewBuilding(1, request));

            Assert.Equal("Building type required.", exception.Message);
        }

        [Fact]
        public void CreateNewBuilding_WithIncorrectInputString_ThrowsException()
        {
            var request = new BuildingRequestDTO
            {
                Type = "Church"
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest2")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.CreateNewBuilding(1, request));

            Assert.Equal("Incorrect building type.", exception.Message);
        }

        [Fact]
        public void CreateNewBuildingAcademy_WithoutFarm_ThrowsException()
        {
            var request = new BuildingRequestDTO
            {
                Type = "Academy"
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest3")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>()
            };
            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.CreateNewBuilding(1, request));

            Assert.Equal("You need a farm to be able to construct an Academy.", exception.Message);
        }

        [Fact]
        public void CreateNewBuildingAcademy_WithoutMine_ThrowsException()
        {
            var request = new BuildingRequestDTO
            {
                Type = "Academy"
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest4")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Type = BuildingType.Farm,
                        BuildingId = 1,
                        KingdomId = 1
                    }
                }
            };
            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.CreateNewBuilding(1, request));

            Assert.Equal("You need a mine to be able to construct an Academy.", exception.Message);
        }

        [Fact]
        public void CreateNewBuilding_WithInsufficientGold_ThrowsException()
        {
            var request = new BuildingRequestDTO
            {
                Type = "Townhall"
            };

            var KingdomGold = new Resource
            {
                Type = ResourceType.Gold,
                Amount = 50,
                KingdomId = 1
            };

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>(),
                Resources = new List<Resource>
                {
                    KingdomGold
                }
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var requestedBuilding = BuildingType.TownHall;
            ruleServiceMock.Setup(x => x.GetBuildingDetails(requestedBuilding, 1, 1)).Returns(new BuildingDetailsDTO
            {
                BuildingDuration = 60,
                BuildingHP = 50,
                BuildingPrice = 100
            });
            timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest5")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.CreateNewBuilding(1, request));

            Assert.Equal("Gold needed.", exception.Message);
        }

        [Fact]
        public void CreateNewBuilding_WithCorrectInputs_ReturnsCorrectValuesAndDeductsGold()
        {
            var request = new BuildingRequestDTO
            {
                Type = "Townhall"
            };

            var KingdomGold = new Resource
            {
                Type = ResourceType.Gold,
                Amount = 200,
                KingdomId = 1
            };

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>(),
                Resources = new List<Resource>
                {
                    KingdomGold
                }
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var requestedBuilding = BuildingType.TownHall;
            ruleServiceMock.Setup(x => x.GetBuildingDetails(requestedBuilding, 1, 1)).Returns(new BuildingDetailsDTO
            {
                BuildingDuration = 60,
                BuildingHP = 50,
                BuildingPrice = 100
            });
            timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest6")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            var expectedTownhall = new Building
            {
                Type = BuildingType.TownHall,
                Level = 1,
                Started_at = 0,
                Finished_at = 60,
                Hp = 50,
                BuildingId = 1,
                KingdomId = 1
            };

            var result =
                new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object).CreateNewBuilding(1,
                    request);

            Assert.Equal("TownHall", result.Type);
            Assert.Equal(expectedTownhall.Started_at, result.Started_at);
            Assert.Equal(expectedTownhall.Finished_at, result.Finished_at);
            Assert.Equal(expectedTownhall.Hp, result.Hp);
            Assert.Equal(expectedTownhall.Level, result.Level);
            Assert.Equal(100, KingdomGold.Amount);



        }

        [Fact]
        public void UpgradeBuilding_WithIncorrectID_ThrowsException()
        {
            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Type = BuildingType.Farm,
                        BuildingId = 1,
                        KingdomId = 1
                    }
                },
                Resources = new List<Resource>()
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest7")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.UpgradeBuilding(1, 5));

            Assert.Equal("Building does not exist!", exception.Message);
        }

        [Fact]
        public void UpgradeBuilding_WithLowLevelTownhall_ThrowsException()
        {
            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Type = BuildingType.TownHall,
                        BuildingId = 1,
                        KingdomId = 1,
                        Level = 1
                    },
                    new Building
                    {
                        Type = BuildingType.Farm,
                        BuildingId = 2,
                        KingdomId = 1,
                        Level = 1
                    }
                },
                Resources = new List<Resource>()
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest8")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            context.SaveChanges();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.UpgradeBuilding(1, 2));

            Assert.Equal("Townhall level is too low!", exception.Message);
        }

        [Fact]
        public void UpgradeBuilding_WithInsufficientGold_ThrowsException()
        {
            var kingdomGold = new Resource
            {
                Type = ResourceType.Gold,
                Amount = 50,
                KingdomId = 1
            };

            var kingdomFood = new Resource
            {
                Type = ResourceType.Food,
                Amount = 200,
                KingdomId = 1
            };

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Type = BuildingType.TownHall,
                        BuildingId = 1,
                        KingdomId = 1,
                        Level = 2
                    },
                    new Building
                    {
                        Type = BuildingType.Farm,
                        BuildingId = 2,
                        KingdomId = 1,
                        Level = 1
                    }
                },
                Resources = new List<Resource>()
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            ruleServiceMock.Setup(x => x.GetBuildingDetails(BuildingType.Farm, 2, 1)).Returns(new BuildingDetailsDTO
            {
                BuildingPrice = 200,
                BuildingHP = 200,
                BuildingDuration = 120
            });

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest9")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            kingdom.Resources.Add(kingdomFood);
            kingdom.Resources.Add(kingdomGold);
            context.SaveChanges();

            var controller = new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object);
            var exception = Record.Exception(() => controller.UpgradeBuilding(1, 2));

            Assert.Equal("You don't have enough gold!", exception.Message);
        }

        [Fact]
        public void UpgradeBuilding_WithCorrectInput_ReturnsCorrectBuilding()
        {
            var kingdomGold = new Resource
            {
                Type = ResourceType.Gold,
                Amount = 300,
                KingdomId = 1
            };

            var kingdomFood = new Resource
            {
                Type = ResourceType.Food,
                Amount = 200,
                KingdomId = 1
            };

            var kingdom = new Kingdom
            {
                KingdomId = 1,
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Type = BuildingType.TownHall,
                        BuildingId = 1,
                        KingdomId = 1,
                        Level = 2
                    },
                    new Building
                    {
                        Type = BuildingType.Farm,
                        BuildingId = 2,
                        KingdomId = 1,
                        Level = 1
                    }
                },
                Resources = new List<Resource>()
            };

            Mock<ITimeService> timeServiceMock = new Mock<ITimeService>();
            Mock<IRulesService> ruleServiceMock = new Mock<IRulesService>();

            timeServiceMock.Setup(x => x.GetCurrentSeconds()).Returns(0);

            ruleServiceMock.Setup(x => x.GetBuildingDetails(BuildingType.Farm, 2, 1)).Returns(new BuildingDetailsDTO
            {
                BuildingPrice = 200,
                BuildingHP = 200,
                BuildingDuration = 120
            });

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("BuildingTest10")
                .Options;

            var context = new ApplicationContext(optionsBuilder);

            context.Kingdoms.Add(kingdom);
            kingdom.Resources.Add(kingdomFood);
            kingdom.Resources.Add(kingdomGold);
            context.SaveChanges();

            var expectedResult = new BuildingResponseDTO
            {
                Started_at = 0,
                Finished_at = 120,
                Hp = 200,
                Id = 2,
                Level = 2,
                Type = "Farm"
            };
            var result =
                new BuildingsService(context, timeServiceMock.Object, ruleServiceMock.Object).UpgradeBuilding(1, 2);

            Assert.Equal(expectedResult.Started_at, result.Started_at);
            Assert.Equal(expectedResult.Finished_at, result.Finished_at);
            Assert.Equal(expectedResult.Hp, result.Hp);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Type, result.Type);
        }

    }
}