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
        private readonly IKingdomService _kingdomService;

        public BattleService(ApplicationContext applicationContext, ITroopService troopService, IKingdomService kingdomService)
        {
            _applicationContext = applicationContext;
            _troopService = troopService;
            _kingdomService = kingdomService;
        }

        public Battle Attack(int myKingdomId, int enemyKingdomId, TroopUpgradeRequestDTO troopUpgradeRequestDto)
        {
            var myKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == myKingdomId);
            var enemyKingdom = _applicationContext.Kingdoms.Single(k => k.KingdomId == enemyKingdomId);

            var myTroops = _troopService.GetReadyTroops(myKingdomId);
            var enemyTroops = _troopService.GetReadyTroops(enemyKingdomId);

            if (troopUpgradeRequestDto.TroopIds.Any(troopId => myTroops.Any(t => t.TroopId != troopId)))
            {
                throw new TroopCreationException("Wrong troops ids!");
            }

            while (!myTroops.Any() && !enemyTroops.Any())
            {
                
            }

            return null;
        }

        private Troop Fight(Troop myTroop, Troop enemyTroop)
        {
            var hitChance = new Random().Next(100);
            while (myTroop.TroopHP != 0 && enemyTroop.TroopHP != 0)
            {
                
            }

            return null;
        }
    }
}