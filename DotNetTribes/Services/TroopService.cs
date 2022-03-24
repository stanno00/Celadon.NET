using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DotNetTribes.Services
{
    public class TroopService : ITroopService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IRulesService _rules;
        private readonly ITimeService _timeService;
        private readonly IResourceService _resourceService;

        public TroopService(ApplicationContext applicationContext, IRulesService rules, ITimeService timeService, IResourceService resourceService)
        {
            _applicationContext = applicationContext;
            _rules = rules;
            _timeService = timeService;
            _resourceService = resourceService;
        }

        public TroopResponseDTO TrainNewTroops(int kingdomId, TroopRequestDTO request)
        {
            // this large DB call is done to avoid making 5 different, smaller DB calls needed throughout the process.
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .FirstOrDefault(k => k.KingdomId == kingdomId);
            var kingdomGold = kingdom!.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold);
            var orderPrice = _rules.TroopPrice(1) * request.NumberOfTroops;
            var troopsInProgress = kingdom.Troops
                .Where(t => t.FinishedAt > _timeService.GetCurrentSeconds())
                .ToList();
            PerformTrainingChecks(kingdom, request.NumberOfTroops, kingdomGold!.Amount, orderPrice);

            var newTroops = CreateNewTroops(kingdomId, request.NumberOfTroops, troopsInProgress);

            foreach (var troop in newTroops)
            {
                kingdom.Troops.Add(troop);
            }

            kingdomGold.Amount -= orderPrice;
            _applicationContext.SaveChanges();

            return new TroopResponseDTO
            {
                NewTroops = newTroops
            };
        }

        public void UpdateTroops(int kingdomId)
        {
            var troopsInProgress = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId && t.FinishedAt < _timeService.GetCurrentSeconds())
                .OrderBy(t => t.FinishedAt)
                .ToList();

            if (troopsInProgress.Count != 0)
            {
                foreach (var troop in troopsInProgress)
                {
                    if (troop.FinishedAt < _timeService.GetCurrentSeconds())
                    {
                        int level = troop.Upgrading ? (troop.Level + 1) : troop.Level;
                        troop.ConsumingFood = true;
                        troop.Level = level;
                        troop.Attack = _rules.TroopAttack(level);
                        troop.Defense = _rules.TroopDefense(level);
                        troop.Capacity = _rules.TroopCapacity(level);
                        // this might not work, test if it behaves correctly
                        troop.Upgrading = false;
                    }
                }

                //Once a soldier (or a bunch of them) are created, they start eating. Fortunately, the methods are implemented in such a way that
                //unless enough time has passed, only the generation will be updated, not the actual amount.
                _resourceService.UpdateKingdomResources(kingdomId);
                _applicationContext.SaveChanges();
            }
        }

        private int CalculateStorageLimit(Kingdom kingdom)
    {
        //TODO : ask about requirements for townhall - forgot to build it on a new account and got nullpointerException here
        return _rules.StorageLimit(kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.TownHall)!.Level) - (kingdom.Troops.Count);
    }

    private void PerformTrainingChecks(Kingdom kingdom, int requestedAmount, int goldAmount, int orderPrice)
    {
        if (kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.Academy) == null)
        {
            throw new TroopCreationException("You need an academy to be able to train troops.");
        }

        if (CalculateStorageLimit(kingdom) < requestedAmount)
        {
            throw new TroopCreationException("Insufficient troop capacity.");
        }

        if (orderPrice > goldAmount)
        {
            throw new TroopCreationException("Not enough gold.");
        }
    }

    private List<Troop> CreateNewTroops(int kingdomId, int amount, List<Troop> troopsInProgress)
    {
        var newTroops = new List<Troop>();

        for (int i = 0; i < amount; i++)
        {
            newTroops.Add(new Troop
            {
                StartedAt = troopsInProgress.Count == 0 ? _timeService.GetCurrentSeconds() + i * _rules.TroopBuildingTime(1) : troopsInProgress.Last().FinishedAt + i * _rules.TroopBuildingTime(1),
                FinishedAt =
                    troopsInProgress.Count == 0 ? _timeService.GetCurrentSeconds() + (i + 1) * _rules.TroopBuildingTime(1) : troopsInProgress.Last().FinishedAt + (i + 1) * _rules.TroopBuildingTime(1),
                Level = 1,
                Attack = _rules.TroopAttack(1),
                Defense = _rules.TroopDefense(1),
                Capacity = _rules.TroopCapacity(1),
                ConsumingFood = false,
                Upgrading = false,
                KingdomId = kingdomId
            });
        }
        
        return newTroops;
    }

    public KingdomTroopsDTO GetKingdomTroops(int kingdomId)
    {
        return new KingdomTroopsDTO
        {
            Troops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId)
                .ToList()
        };
    }
/*
    public KingdomTroopsDTO UpgradeTroops(int kingdomId, TroopUpgradeRequestDTO request)
    {
        var troopsToUpgrade = PerformUpgradeChecksAndReturnTroopsToUpgrade(kingdomId, request);
        List<Troop> troopsForResponse = new List<Troop>();

        foreach (var troop in troopsToUpgrade)
        {
            var level = troop.Level + 1;
            troopsForResponse.Add(new Troop
            {
                KingdomId = kingdomId,
                Level = level,
                Attack = _rules.TroopAttack(level),
                Defense = _rules.TroopDefense(level),
                Capacity = _rules.TroopCapacity(level),
            });
        }

        var responseDTO = new KingdomTroopsDTO(
        )
    }*/

    private List<Troop> PerformUpgradeChecksAndReturnTroopsToUpgrade(int kingdomId, TroopUpgradeRequestDTO request)
    {
        //Check that the troops we want to upgrade actually belong to the kingdom
        List<Troop> troopsToUpgrade = new List<Troop>();
        foreach (var troopId in request.TroopIds)
        {
            var troop = _applicationContext.Troops.FirstOrDefault(t => t.TroopId == troopId);
            if (troop.KingdomId != kingdomId)
            {
                throw new TroopCreationException("Invalid Troop ID.");
            }

            troopsToUpgrade.Add(troop);
        }

        troopsToUpgrade = troopsToUpgrade.OrderBy(t => t.Level).ToList();

        //Check that the best troop's level after upgrade isn't higher than the academy's
        if ((troopsToUpgrade[0].Level + 1) > _applicationContext.Buildings.FirstOrDefault(b => b.Type == BuildingType.Academy && b.KingdomId == kingdomId).Level)
        {
            throw new TroopCreationException("Upgrade not allowed, academy level too low.");
        }

        return troopsToUpgrade;
    }
}

}