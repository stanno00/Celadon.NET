using System.Collections.Generic;
using DotNetTribes;
using DotNetTribes.DTOs;
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
    public void UpgradeService_AddUpgrade_ErrorUpgradeExist()
    {
        //Arrange
        Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ErrorUpgradeExist")
            .Options;

        var context = new ApplicationContext(optionsBuilder);
        var controller = new UpgradeService(context, iRuleServiceMock.Object, iTimeServiceMock.Object);

        context.Kingdoms.Add(new Kingdom()
        {
            Buildings = new List<Building>(),
            Resources = new List<Resource>(),
            BuildingUpgrade = new List<BuildingUpgrade>()
                {new BuildingUpgrade() {Name = AllBuildingUpgrades.Scout, KingdomId = 1, BuildingUpgradeId = 1}},
            KingdomId = 1,
        });
        context.SaveChanges();
        
        var upgrade = new BuildingsUpgradesRequestDto()
        {
            UpgradeName = AllBuildingUpgrades.Scout
        };

        //Act
        var exception = Record.Exception(() => controller.AddUpgrade(1, upgrade));

        //Assert
        Assert.Equal("Building already have this upgrade", exception.Message);
    }

    [Fact]
    public void UpgradeService_AddUpgrade_ErrorNoGold()
    {
        //Arrange
        Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ErrorNoGold")
            .Options;

        var context = new ApplicationContext(optionsBuilder);
        var controller = new UpgradeService(context, iRuleServiceMock.Object, iTimeServiceMock.Object);

        context.Kingdoms.Add(new Kingdom()
        {
            Buildings = new List<Building>(),
            Resources = new List<Resource>()
            {
                new Resource() {Amount = 1, KingdomId = 1, ResourceId = 1, Type = ResourceType.Gold},
                new Resource() {Amount = 1000, KingdomId = 1, ResourceId = 2, Type = ResourceType.Food}
            },
            BuildingUpgrade = new List<BuildingUpgrade>(),
            KingdomId = 1,
        });
        context.SaveChanges();
        
        var upgrade = new BuildingsUpgradesRequestDto()
        {
            UpgradeName = AllBuildingUpgrades.Scout
        };

        //Act
        var exception = Record.Exception(() => controller.AddUpgrade(1, upgrade));

        //Assert
        Assert.Equal("You don't have enough gold!", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_AddUpgrade_ErrorNoFood()
    {
        //Arrange
        Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ErrorNoFood")
            .Options;

        var context = new ApplicationContext(optionsBuilder);
        var controller = new UpgradeService(context, iRuleServiceMock.Object, iTimeServiceMock.Object);

        context.Kingdoms.Add(new Kingdom()
        {
            Buildings = new List<Building>(),
            Resources = new List<Resource>()
            {
                new Resource() {Amount = 1000, KingdomId = 1, ResourceId = 1, Type = ResourceType.Gold},
                new Resource() {Amount = 1, KingdomId = 1, ResourceId = 2, Type = ResourceType.Food}},
            BuildingUpgrade = new List<BuildingUpgrade>(),
            KingdomId = 1,
        });
        context.SaveChanges();
        var upgrade = new BuildingsUpgradesRequestDto()
        {
            UpgradeName = AllBuildingUpgrades.Scout
        };

        //Act
        var exception = Record.Exception(() => controller.AddUpgrade(1, upgrade));

        //Assert
        Assert.Equal("You don't have enough food!", exception.Message);
    }
    
    [Fact]
    public void UpgradeService_AddUpgrade_ReturnUpgrade()
    {
        //Arrange
        Mock<IRulesService> iRuleServiceMock = new Mock<IRulesService>();
        Mock<ITimeService> iTimeServiceMock = new Mock<ITimeService>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("ReturnUpgrade")
            .Options;

        var context = new ApplicationContext(optionsBuilder);
        var controller = new UpgradeService(context, iRuleServiceMock.Object, iTimeServiceMock.Object);

        context.Kingdoms.Add(new Kingdom()
        {
            Buildings = new List<Building>(),
            Resources = new List<Resource>()
            {
                new Resource() {Amount = 1000, KingdomId = 1, ResourceId = 1, Type = ResourceType.Gold},
                new Resource() {Amount = 1000, KingdomId = 1, ResourceId = 2, Type = ResourceType.Food}},
            BuildingUpgrade = new List<BuildingUpgrade>(),
            KingdomId = 1,
        });
        context.SaveChanges();
        
        var upgrade = new BuildingsUpgradesRequestDto()
        {
            UpgradeName = AllBuildingUpgrades.Scout
        };

        //Act
        var result = controller.AddUpgrade(1, upgrade);
        
        //Assert
        Assert.IsType<BuildingsUpgradesResponseDto>(result);
        Assert.Equal("Scout",result.Name);
        Assert.Equal(1,result.KingdomId);
    }
}