using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNetTribesTests.Unit;

public class BattleServiceTest
{
    [Fact]
    public void Initialize_Battle_CorrectValues()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("BattleDatabase")
            .Options;

        var context = new ApplicationContext(options);

        var troops = context.Troops.Add(new Troop
        {
            TroopId = 1,
            KingdomId = 1
        });

        var attackerKingdom = context.Kingdoms.Add(new Kingdom()
        {
            KingdomId = 1,
            KingdomX = 0,
            KingdomY = 0
        });
        
        var defenderKingdom = context.Kingdoms.Add(new Kingdom()
        {
            KingdomId = 2,
            KingdomX = 1,
            KingdomY = 1
        });

        var troopsToValidate = new TroopUpgradeRequestDTO()
        {
            TroopIds = new List<long>() {1}
        };

        context.SaveChanges();

        var troopServiceMock = new Mock<ITroopService>();
        var ruleServiceMock = new Mock<IRulesService>();
        var timeServiceMock = new Mock<ITimeService>();
        var kingDomServiceMock = new Mock<IKingdomService>();
        var resourceServiceMock = new Mock<IResourceService>();

        var battleService = new BattleService(context,
            troopServiceMock.Object,
            ruleServiceMock.Object,
            timeServiceMock.Object,
            kingDomServiceMock.Object,
            resourceServiceMock.Object);

        troopServiceMock.Setup(t => t.GetReadyTroops(attackerKingdom.Entity.KingdomId))
            .Returns(new List<Troop> {troops.Entity});

        timeServiceMock.Setup(t => t.GetCurrentSeconds()).Returns(0);
        kingDomServiceMock.Setup(k => k.ShortestPath(attackerKingdom.Entity.KingdomX,
            attackerKingdom.Entity.KingdomY,
            defenderKingdom.Entity.KingdomX,
            defenderKingdom.Entity.KingdomY)).Returns(2);

        //Act
        var result = battleService.InitializeBattle(attackerKingdom.Entity.KingdomId, defenderKingdom.Entity.KingdomId,
            troopsToValidate);

        Assert.Equal(1, result.AttackerId);
        Assert.Equal(2, result.DefenderId);
        Assert.Equal(120, result.ArriveAt);
        Assert.Equal(240, result.ReturnAt);
    }
}