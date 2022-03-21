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

            //TODO: try to find a way to cut the kingdom out in this part
            var troops = _applicationContext.Troops
                .Where(t => t.KingdomId == kingdomId)
                .ToList();
            
            var troopsWorkedOn = _applicationContext.TroopsWorkedOn
                .Where(t => t.KingdomId == kingdomId)
                .ToList();
            
            var storageLimit = _rules.StorageLimit(townhall.Level) - (troops.Count + troopsWorkedOn.Count) ;

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

            // add queue mechanic
            for (int i = 0; i < request.Number_of_troops; i++)
            {
                newTroops.Add(new UnfinishedTroop
                {
                    StartedAt = _timeService.GetCurrentSeconds(),
                    FinishedAt = _timeService.GetCurrentSeconds() + _rules.TroopBuildingTime(1),
                });
            }

            foreach (var troop in newTroops)
            {
                troopsWorkedOn.Add(troop);
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
                      ConsumingFood = true
                  });
                  troopsWorkedOn.Remove(troop);
              }
              troop.UpdatedAt = _timeService.GetCurrentSeconds();
            }

            _applicationContext.SaveChanges();
        }
    }
}