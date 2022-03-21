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
            Building townhall = _applicationContext.Buildings.FirstOrDefault(b => b.KingdomId == kingdomId && b.Type == BuildingType.TownHall);

            if (townhall == null)
            {
                throw new TroopCreationException("You need a townhall first!");
            }

            Building academy = _applicationContext.Buildings.FirstOrDefault(b => b.KingdomId == kingdomId && b.Type == BuildingType.Academy);

            if (academy == null)
            {
                throw new TroopCreationException("You need an academy to be able to train troops.");
            }

            //TODO: move this to a separate method
            var troops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId)
                .ToList();

            var troopsWorkedOn = _applicationContext.TroopsWorkedOn
                .Where(t => t.KingdomId == kingdomId)
                .ToList();

            var storageLimit = _rules.StorageLimit(townhall.Level) - (troops.Count + troopsWorkedOn.Count);

            if (storageLimit < request.Number_of_troops)
            {
                throw new TroopCreationException("Insufficient troop capacity.");
            }

            var kingdomGold = _applicationContext.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold && r.KingdomId == kingdomId);
            var orderPrice = _rules.TroopPrice(1) * request.Number_of_troops;

            if (orderPrice > kingdomGold!.Amount)
            {
                throw new TroopCreationException("Not enough gold.");
            }

            List<UnfinishedTroop> newTroops = new List<UnfinishedTroop>();

            // if the queue is empty:
            {
                for (int i = 0; i < request.Number_of_troops; i++)
                {
                    newTroops.Add(new UnfinishedTroop
                    {
                        //The starting time is decided on whether there are other troops being worked on. If there aren't, training starts now. If there are, it starts as soon as the last troop is finished training.
                        StartedAt = troopsWorkedOn.Count == 0 ? 
                            _timeService.GetCurrentSeconds() + i * _rules.TroopBuildingTime(1) : 
                            troopsWorkedOn.Last().FinishedAt +  i * _rules.TroopBuildingTime(1),
                        FinishedAt =
                            troopsWorkedOn.Count == 0 ? 
                                _timeService.GetCurrentSeconds() + (i + 1) * _rules.TroopBuildingTime(1) :
                                troopsWorkedOn.Last().FinishedAt +  (i + 1) * _rules.TroopBuildingTime(1),
                        Level = 1,
                        KingdomId = kingdomId
                    });
                }
            }

            foreach (var troop in newTroops)
            {
                _applicationContext.TroopsWorkedOn.Add(troop);
            }

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
    }
}