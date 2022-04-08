using System;
using System.Collections.Generic;
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
        private readonly IKingdomService _kingdomService;

        public BattleService(ApplicationContext applicationContext, ITroopService troopService, IRulesService rulesService, ITimeService timeService, IKingdomService kingdomService)
        {
            _applicationContext = applicationContext;
            _troopService = troopService;
            _rulesService = rulesService;
            _timeService = timeService;
            _kingdomService = kingdomService;
        }

        public Battle Attack(int myKingdomId, int enemyKingdomId, TroopUpgradeRequestDTO troopUpgradeRequestDto)
        {
            // Getting available troops from kingdom
            var attackerReadyTroops = _troopService.GetReadyTroops(myKingdomId);

            // Checking if requested troops exist and if so add them to a new list
            var attackerBattleTroops = new List<Troop>();
            foreach (var troop in troopUpgradeRequestDto.TroopIds.Select(troopId => attackerReadyTroops.FirstOrDefault(t => t.TroopId == troopId)))
            {
                if (troop == null)
                {
                    throw new TroopCreationException("Wrong troop ids");
                }
                attackerBattleTroops.Add(troop);
            }

            // Getting all available defending troops and their count
            var defenderTroops = _troopService.GetReadyTroops(enemyKingdomId);
            var defenderArmyCountBeforeBattle = defenderTroops.Count;

            var myKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == myKingdomId);
            var enemyKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == enemyKingdomId);

            // Sending attacker troops for the attack so they can't be used in another battle
            attackerBattleTroops.ForEach(t => t.IsHome = false);
            
            // Calculating attack duration in seconds
            var attackDuration = _kingdomService.ShortestPath(myKingdom.KingdomX, myKingdom.KingdomY,
                enemyKingdom.KingdomX, enemyKingdom.KingdomY) * 60;

            // Determine the battle winner and remove dead troops from either list and database
            while (attackerBattleTroops.Any() && defenderTroops.Any())
            {
                var loser = Fight(attackerBattleTroops[0], defenderTroops[0]);
                if (loser == attackerBattleTroops[0])
                {
                    attackerBattleTroops.Remove(loser);
                }

                defenderTroops.Remove(loser);
                _applicationContext.Troops.Remove(loser);
            }
            
            var winner = attackerBattleTroops.Count == 0 ? enemyKingdomId : myKingdomId;

            // Calculating losses
            var attackerLostTroops = troopUpgradeRequestDto.TroopIds.Count - attackerBattleTroops.Count;
            var defenderLostTroops = defenderArmyCountBeforeBattle - defenderTroops.Count;

            var battleResult = new Battle
            {
                ArriveAt = _timeService.GetCurrentSeconds() + attackDuration / 2,
                AttackerId = myKingdomId,
                DefenderId = enemyKingdomId,
                FightStart = _timeService.GetCurrentSeconds() + attackDuration / 2,
                FoodStolen = 0,
                GoldStolen = 0,
                LostTroopsAttacker = attackerLostTroops,
                LostTroopsDefender = defenderLostTroops,
                ReturnAt = _timeService.GetCurrentSeconds() + attackDuration,
                WinnerId = winner
            };
            
            // Using updatedAt to return troops to kingdom
            foreach (var attackerBattleTroop in attackerBattleTroops)
            {
                attackerBattleTroop.UpdatedAt = battleResult.ReturnAt;
            }

            _applicationContext.SaveChanges();

            return battleResult;
        }

        // Fight between 2 troops
        // Returning defeated troop so it can be removed from list and DB
        private Troop Fight(Troop myTroop, Troop enemyTroop)
        {
            while (myTroop.TroopHP > 0 && enemyTroop.TroopHP > 0)
            {
                enemyTroop.TroopHP -= DamageDone(myTroop, enemyTroop);
                if(enemyTroop.TroopHP > 0)
                {
                    myTroop.TroopHP -= DamageDone(enemyTroop, myTroop);
                }                
            }

            var loser = myTroop.TroopHP <= 0 ? myTroop : enemyTroop;
            return loser;
        }

        //Calculation damage done for each attack
        private int DamageDone(Troop troopAttacking, Troop troopDefending)
        {
            var hitChance = new Random().Next(100);
            if (hitChance < 30)
            {
                return 0;
            }

            var criticalChance = new Random().Next(100);
            if (criticalChance > 85)
            {
                var doubleDamage = _rulesService.TroopAttack(troopAttacking.Level) * 2 - _rulesService.TroopDefense(troopDefending.Level);
                return doubleDamage;
            }

            var damage = _rulesService.TroopAttack(troopAttacking.Level) - _rulesService.TroopDefense(troopDefending.Level);
            return damage;
        }
    }
}