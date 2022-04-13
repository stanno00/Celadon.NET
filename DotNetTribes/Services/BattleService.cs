using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
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
        private readonly IResourceService _resourceService;

        public BattleService(ApplicationContext applicationContext, ITroopService troopService, IRulesService rulesService, ITimeService timeService, IKingdomService kingdomService, IResourceService resourceService)
        {
            _applicationContext = applicationContext;
            _troopService = troopService;
            _rulesService = rulesService;
            _timeService = timeService;
            _kingdomService = kingdomService;
            _resourceService = resourceService;
        }
        
        // Getting available troops from kingdom
        // Checking if requested troops are available in the attacking kingdom and if so add them to a new list
        private IEnumerable<Troop> ValidatedTroopsForBattle(int kingdomId, TroopUpgradeRequestDTO troopsRequestedForBattleDto)
        {
            var attackerReadyTroops = _troopService.GetReadyTroops(kingdomId);
            
            var attackerBattleTroops = new List<Troop>();
            foreach (var troop in troopsRequestedForBattleDto.TroopIds.Select(troopId => attackerReadyTroops.FirstOrDefault(t => t.TroopId == troopId)))
            {
                if (troop == null)
                {
                    throw new TroopCreationException("Wrong troop ids");
                }
                attackerBattleTroops.Add(troop);
            }

            return attackerBattleTroops;
        }

        public Battle InitializeBattle(int attackerKingdomId, int defenderKingdomId, TroopUpgradeRequestDTO troopsRequestedForBattleDto)
        {
            var attackerTroops = ValidatedTroopsForBattle(attackerKingdomId, troopsRequestedForBattleDto);
            // Getting kingdoms to calculate distance and time for attack
            var attackingKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == attackerKingdomId);
            var defendingKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == defenderKingdomId);

            // Calculating attack duration in seconds
            var timeToReachDefendingKingdom = _kingdomService.ShortestPath(attackingKingdom.KingdomX, attackingKingdom.KingdomY,
                defendingKingdom.KingdomX, defendingKingdom.KingdomY) * 60;
            
            // Initializing battle
            var battle = new Battle
            {
                ArriveAt = _timeService.GetCurrentSeconds() + timeToReachDefendingKingdom,
                AttackerId = attackerKingdomId,
                DefenderId = defenderKingdomId,
                ReturnAt = _timeService.GetCurrentSeconds() + timeToReachDefendingKingdom * 2,
            };

            _applicationContext.Add(battle);
            _applicationContext.SaveChanges();

            // Assigning battle to each troop and setting return time
            foreach (var troop in attackerTroops)
            {
                troop.BattleId = battle.BattleId;
                troop.ReturnedFromBattleAt = battle.ReturnAt;
            }
            
            _applicationContext.SaveChanges();

            return battle;
        }

        public Battle UpdateBattles()
        {
            var currentBattles = _applicationContext.Battles.Where(b => b.ArriveAt < _timeService.GetCurrentSeconds() && b.WinnerId == 0).ToList();

            foreach (var battle in currentBattles)
            {
                battle.FightStart = _timeService.GetCurrentSeconds();
                // Getting both armies and their count
                var defenderTroops = _troopService.GetReadyTroops(battle.DefenderId);
                var defenderArmyCountBeforeBattle = defenderTroops.Count;
                var attackerTroops = _applicationContext.Troops.Where(t => t.BattleId == battle.BattleId).ToList();
                var attackerTroopsCountBeforeBattle = attackerTroops.Count;
                var defenderFoodAmountBeforeBattle = _resourceService.GetResourceAmount(battle.DefenderId, ResourceType.Food);
                var defenderGoldAmountBeforeBattle = _resourceService.GetResourceAmount(battle.DefenderId, ResourceType.Gold);

                // Determine the battle winner and remove dead troops from either list and database
                while (attackerTroops.Any() && defenderTroops.Any())
                {
                    var defeatedTroop = Fight(attackerTroops[0], defenderTroops[0]);
                    if (defeatedTroop == attackerTroops[0])
                    {
                        attackerTroops.Remove(defeatedTroop);
                    }

                    defenderTroops.Remove(defeatedTroop);
                    _applicationContext.Troops.Remove(defeatedTroop);
                }

                var winner = attackerTroops.Count == 0 ? battle.DefenderId : battle.AttackerId;

                // Calculating troop losses
                var attackerLostTroops = attackerTroopsCountBeforeBattle - attackerTroops.Count;
                var defenderLostTroops = defenderArmyCountBeforeBattle - defenderTroops.Count;
                
                StealResources(attackerTroops, battle.DefenderId);

                var defenderFoodAmountAfterBattle = _resourceService.GetResourceAmount(battle.DefenderId, ResourceType.Food);
                var defenderGoldAmountAfterBattle = _resourceService.GetResourceAmount(battle.DefenderId, ResourceType.Gold);

                // Updating battle entity
                battle.FoodStolen = defenderFoodAmountBeforeBattle - defenderFoodAmountAfterBattle;
                battle.GoldStolen = defenderGoldAmountBeforeBattle - defenderGoldAmountAfterBattle;
                battle.WinnerId = winner;
                battle.LostTroopsAttacker = attackerLostTroops;
                battle.LostTroopsDefender = defenderLostTroops;
                
                _applicationContext.SaveChanges();
                return battle;
            }

            return null;
        }

        // Fight between 2 troops
        // Returning defeated troop so it can be removed from list and database
        private Troop Fight(Troop attackingTroop, Troop defendingTroop)
        {
            while (attackingTroop.TroopHP > 0 && defendingTroop.TroopHP > 0)
            {
                defendingTroop.TroopHP -= DamageDone(attackingTroop, defendingTroop);
                if(defendingTroop.TroopHP > 0)
                {
                    attackingTroop.TroopHP -= DamageDone(defendingTroop, attackingTroop);
                }                
            }

            var defeatedTroop = attackingTroop.TroopHP <= 0 ? attackingTroop : defendingTroop;
            return defeatedTroop;
        }

        //Calculating damage done with each attack
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
                var doubleDamage = _rulesService.TroopAttack(troopAttacking.Level, troopAttacking.KingdomId) * 2 
                                   - _rulesService.TroopDefense(troopDefending.Level, troopAttacking.KingdomId);
                return doubleDamage;
            }

            var damage = _rulesService.TroopAttack(troopAttacking.Level, troopAttacking.KingdomId) 
                         - _rulesService.TroopDefense(troopDefending.Level, troopAttacking.KingdomId);
            return damage;
        }

        private void StealResources(IEnumerable<Troop> attackerTroops, int kingdomId)
        {
            // Semi-working code. I have no idea how to fill capacity and stay bellow 50% of stolen resources
            var troopsResourceCapacity = attackerTroops.Select(t => t.Capacity).Sum();
            var resources = _applicationContext.Resources
                .Where(r => r.KingdomId == kingdomId && r.Type != ResourceType.Iron)
                .ToList();
            
            foreach (var resource in resources)
            {
                var maxAmountToSteal = resource.Amount / 2;
                var resourcesToSteal = troopsResourceCapacity / resources.Count;
                
                if (resourcesToSteal > maxAmountToSteal)
                {
                    resource.Amount -= maxAmountToSteal;
                }
                else
                {
                    resource.Amount -= resourcesToSteal;
                }

                if (resource.Amount < 0)
                {
                    resource.Amount = 0;
                }
                
                
            }
            _applicationContext.SaveChanges();
        }
    }
}