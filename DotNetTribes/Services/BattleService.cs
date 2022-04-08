using System;
using System.Linq;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public class BattleService : IBattleService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ITroopService _troopService;
        private readonly IRulesService _rulesService;
        private readonly ITimeService _timeService;

        public BattleService(ApplicationContext applicationContext, ITroopService troopService, IRulesService rulesService, ITimeService timeService)
        {
            _applicationContext = applicationContext;
            _troopService = troopService;
            _rulesService = rulesService;
            _timeService = timeService;
        }

        public Battle Attack(int myKingdomId, int enemyKingdomId, TroopUpgradeRequestDTO troopUpgradeRequestDto)
        {
            var myTroops = _troopService.GetReadyTroops(myKingdomId);

            if (troopUpgradeRequestDto.TroopIds.Any(troopId => myTroops.Any(t => t.TroopId != troopId)))
            {
                throw new TroopCreationException("Wrong troops ids!");
            }

            var enemyTroops = _troopService.GetReadyTroops(enemyKingdomId);
            var enemyTroopsBeforeBattle = enemyTroops.Count;

            var myKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == myKingdomId);
            var enemyKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == enemyKingdomId);

            myTroops.ForEach(t => t.IsHome = false);

            while (myTroops.Any() && enemyTroops.Any())
            {
                var loser = Fight(myTroops[0], enemyTroops[0]);
                if (loser == myTroops[0])
                {
                    myTroops.Remove(loser);
                }

                enemyTroops.Remove(loser);
            }

            var winner = myTroops.Count == 0 ? enemyKingdomId : myKingdomId;

            var attackerLostTroops = troopUpgradeRequestDto.TroopIds.Count - myTroops.Count;
            var defenderLostTroops = enemyTroopsBeforeBattle - enemyTroops.Count;

            var battleResult = new Battle
            {
                ArriveAt = _timeService.GetCurrentSeconds(),
                AttackerId = myKingdomId,
                DefenderId = enemyKingdomId,
                FightStart = _timeService.GetCurrentSeconds(),
                FoodStolen = 0,
                GoldStolen = 0,
                LostTroopsAttacker = attackerLostTroops,
                LostTroopsDefender = defenderLostTroops,
                ReturnAt = _timeService.GetCurrentSeconds(),
                WinnerId = winner
            };

            myKingdom.Troops = myTroops;
            enemyKingdom.Troops = enemyTroops;

            _applicationContext.SaveChanges();

            return battleResult;
        }

        private Troop Fight(Troop myTroop, Troop enemyTroop)
        {
            while (myTroop.TroopHP != 0 && enemyTroop.TroopHP != 0)
            {
                enemyTroop.TroopHP -= DamageDone(myTroop, enemyTroop);
                myTroop.TroopHP -= DamageDone(enemyTroop, myTroop);
            }

            return myTroop.TroopHP == 0 ? myTroop : enemyTroop;
        }

        private int DamageDone(Troop troopAttacking, Troop troopDefending)
        {
            var hitChance = new Random().Next(100);
            if (hitChance < 30)
            {
                return 0;
            }

            var critChance = new Random().Next(100);
            if (critChance > 85)
            {
                var doubleDamage = _rulesService.TroopAttack(troopAttacking.Level) * 2 - _rulesService.TroopDefense(troopDefending.Level);
                return doubleDamage;
            }

            var damage = _rulesService.TroopAttack(troopAttacking.Level) - _rulesService.TroopDefense(troopDefending.Level);
            return damage;
        }
    }
}