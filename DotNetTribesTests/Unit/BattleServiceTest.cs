using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using DotNetTribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNetTribesTests.Unit;

public class BattleServiceTest
{
    [Fact]
    public void Initialize_Battle_CorrectValues()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("BattleDatabase1")
            .Options;

        var context = new ApplicationContext(options);

        var troops = new Troop
        {
            TroopId = 1,
            KingdomId = 1
        };

        var attackerKingdom = new Kingdom()
        {
            KingdomId = 1,
        };
        
        var defenderKingdom = new Kingdom()
        {
            KingdomId = 2,
        };

        var troopsToValidate = new TroopUpgradeRequestDTO()
        {
            TroopIds = new List<long>() {1}
        };

        context.Troops.Add(troops);
        context.Kingdoms.Add(attackerKingdom);
        context.Kingdoms.Add(defenderKingdom);
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

        troopServiceMock.Setup(t => t.GetReadyTroops(attackerKingdom.KingdomId))
            .Returns(new List<Troop> {troops});

        timeServiceMock.Setup(t => t.GetCurrentSeconds()).Returns(0);
        
        kingDomServiceMock.Setup(k => k.ShortestPath(attackerKingdom.KingdomX,
            attackerKingdom.KingdomY,
            defenderKingdom.KingdomX,
            defenderKingdom.KingdomY)).Returns(2);

        //Act
        var battle = battleService.InitializeBattle(attackerKingdom.KingdomId, defenderKingdom.KingdomId,
            troopsToValidate);

        Assert.Equal(1, battle.AttackerId);
        Assert.Equal(2, battle.DefenderId);
        Assert.Equal(120, battle.ArriveAt);
        Assert.Equal(240, battle.ReturnAt);
    }

    [Fact]
    public void Initialize_Battle_Exceptions()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("BattleDatabase2")
            .Options;

        var context = new ApplicationContext(options);

        var attackerKingdom = context.Kingdoms.Add(new Kingdom()
        {
            KingdomId = 1,
            Troops = new List<Troop>()
            {
                new Troop
                {
                    TroopId = 1,
                    KingdomId = 1
                }
            }
        });
        
        var defenderKingdom = context.Kingdoms.Add(new Kingdom()
        {
            KingdomId = 2
        });

        var incorrectTroopsToValidate = new TroopUpgradeRequestDTO()
        {
            TroopIds = new List<long>() {2}
        };
        
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
            .Returns((List<Troop>) attackerKingdom.Entity.Troops);

        var incorrectTroops = Record.Exception(() => battleService.InitializeBattle(attackerKingdom.Entity.KingdomId, defenderKingdom.Entity.KingdomId,
            incorrectTroopsToValidate));

        var incorrectDefenderKingdom = Record.Exception(() => battleService.InitializeBattle(
            attackerKingdom.Entity.KingdomId, 3,
            troopsToValidate));

        Assert.NotNull(incorrectTroops);
        Assert.IsType<TroopCreationException>(incorrectTroops);
        Assert.Equal("Wrong troop ids", incorrectTroops.Message);
        
        Assert.NotNull(incorrectDefenderKingdom);
        Assert.IsType<KingdomNotFoundException>(incorrectDefenderKingdom);
        Assert.Equal("Targeted kingdom does not exist!", incorrectDefenderKingdom.Message);
    }

    [Fact]
    public void UpdateBattles_CorrectValues()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("BattleDatabase3")
            .Options;

        var context = new ApplicationContext(options);

        var battle = new Battle()
        {
            BattleId = 1,
            AttackerId = 1,
            DefenderId = 2
        };
        
        for (var i = 0; i < 20; i++)
        {
            context.Troops.Add( new Troop()
            {
                Attack = 10,
                Defense = 5,
                Capacity = 2,
                Level = 1,
                TroopHP = 20,
                BattleId = 1,
                TroopId = i + 1,
                ReturnedFromBattleAt = 241,
                KingdomId = 1
            });
        }

        for (var i = 0; i < 2; i++)
        {
            var troop = new Troop()
            {
                Attack = 10,
                Defense = 5,
                Capacity = 2,
                Level = 1,
                TroopId = i + 200,
                KingdomId = 2,
                TroopHP = 20,
                FinishedAt = 1
            };
            context.Troops.Add(troop);

        }

        var attackerKingdom = new Kingdom()
        {
            KingdomId = 1,
        };
        
        var defenderKingdom = new Kingdom()
        {
            KingdomId = 2,
        };
        
        context.Battles.Add(battle);
        context.Kingdoms.Add(attackerKingdom);
        context.Kingdoms.Add(defenderKingdom);
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

        troopServiceMock.Setup(t => t.GetReadyTroops(battle.DefenderId))
            .Returns(context.Troops
                .Where(t => t.KingdomId == defenderKingdom.KingdomId)
                .ToList);

        ruleServiceMock.Setup(r => r.TroopAttack(1, 1)).Returns(30);
        ruleServiceMock.Setup(r => r.TroopAttack(1, 2)).Returns(0);
        
        ruleServiceMock.Setup(r => r.TroopDefense(1, 1)).Returns(5);
        ruleServiceMock.Setup(r => r.TroopDefense(1, 2)).Returns(5);

        var resolvedBattle = battleService.UpdateBattles();
        
        Assert.Equal(2, resolvedBattle.LostTroopsDefender);
        Assert.Equal(0, resolvedBattle.LostTroopsAttacker);
        Assert.Equal(20, resolvedBattle.FightStart);
    }
}