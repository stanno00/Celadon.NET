using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DotNetTribes.Services
{
    public class TroopService : ITroopService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IRulesService _rules;
        private readonly ITimeService _timeService;

        public TroopService(ApplicationContext applicationContext, IRulesService rules, ITimeService timeService)
        {
            _applicationContext = applicationContext;
            _rules = rules;
            _timeService = timeService;
        }

        public TroopResponseDTO CreateNewTroops(int kingdomId, TroopRequestDTO request)
        {
            // this large DB call is done to avoid making 5 different, smaller DB calls needed throughout the process.
            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .Include(k => k.TroopsWorkedOn)
                .FirstOrDefault(k => k.KingdomId == kingdomId);
            var kingdomGold = kingdom.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold);
            var orderPrice = _rules.TroopPrice(1) * request.Number_of_troops;
            
            PerformChecks(kingdom, request.Number_of_troops, kingdomGold!.Amount,orderPrice);

            List<UnfinishedTroop> newTroops = CreateNewTroops(kingdomId, request.Number_of_troops, kingdom.TroopsWorkedOn.ToList());

            foreach (var troop in newTroops)
            {
                kingdom.TroopsWorkedOn.Add(troop);
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
            var troops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId)
                .ToList();

            var troopsWorkedOn = _applicationContext.TroopsWorkedOn
                .Where(t => t.KingdomId == kingdomId)
                .ToList();

            foreach (var troop in troopsWorkedOn)
            {
                if (troop.FinishedAt < _timeService.GetCurrentSeconds())
                {
                    troops.Add(new Troop
                    {
                        //TODO: Add coordinates once they get implemented
                        KingdomId = kingdomId,
                        ConsumingFood = true,
                        Level = troop.Upgrading ? (troop.Level + 1) : troop.Level
                    });
                    troopsWorkedOn.Remove(troop);
                }

                troop.UpdatedAt = _timeService.GetCurrentSeconds();
            }

            _applicationContext.SaveChanges();
        }

        public int CalculateStorageLimit(Kingdom kingdom)
        {
            return _rules.StorageLimit(kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.TownHall).Level) - (kingdom.Troops.Count + kingdom.TroopsWorkedOn.Count);
        }
        
        public void PerformChecks(Kingdom kingdom, int requestedAmount, int goldAmount, int orderPrice)
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

        public List<UnfinishedTroop> CreateNewTroops(int kingdomId, int amount, List<UnfinishedTroop> troopsWorkedOn)
        {
            List<UnfinishedTroop> newTroops = new List<UnfinishedTroop>();

            for (int i = 0; i < amount; i++)
            {
                newTroops.Add(new UnfinishedTroop
                {
                    //The starting time is decided on whether there are other troops being worked on (Built or upgraded). If there aren't, training starts now. If there are, it starts as soon as the last troop is finished training.
                    StartedAt = troopsWorkedOn.Count == 0 ? _timeService.GetCurrentSeconds() + i * _rules.TroopBuildingTime(1) : troopsWorkedOn.Last().FinishedAt + i * _rules.TroopBuildingTime(1),
                    FinishedAt =
                        troopsWorkedOn.Count == 0 ? _timeService.GetCurrentSeconds() + (i + 1) * _rules.TroopBuildingTime(1) : troopsWorkedOn.Last().FinishedAt + (i + 1) * _rules.TroopBuildingTime(1),
                    Level = 1,
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
        
        public void CheckTrainingFinished()
        {
            
        }
    }
}