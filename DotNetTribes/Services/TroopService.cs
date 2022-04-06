using System.Collections.Generic;
using System.Linq;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Enums;
using DotNetTribes.Exceptions;
using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;


namespace DotNetTribes.Services
{
    public class TroopService : ITroopService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IRulesService _rules;
        private readonly ITimeService _timeService;
        private readonly IResourceService _resourceService;

        public TroopService(ApplicationContext applicationContext, IRulesService rules, ITimeService timeService,
            IResourceService resourceService)
        {
            _applicationContext = applicationContext;
            _rules = rules;
            _timeService = timeService;
            _resourceService = resourceService;
        }

        public TroopResponseDTO TrainTroops(int kingdomId, TroopRequestDTO request)
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

            var newTroops = CreateNewTroops(kingdomId, request.NumberOfTroops, troopsInProgress, null);

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
                        troop.ConsumingFood = true;
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
            return _rules.StorageLimit(kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.TownHall)!.Level) -
                   (kingdom.Troops.Count);
        }

        private void PerformTrainingChecks(Kingdom kingdom, int requestedAmount, int goldAmount, int orderPrice)
        {
            //TODO: add input validation (request == null)
            if (kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.TownHall) == null)
            {
                throw new TroopCreationException("Build a Townhall first!");
            }
            
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

        private List<Troop> CreateNewTroops(int kingdomId, int amount, List<Troop> troopsInProgress, string? name)
        {
            var newTroops = new List<Troop>();

            if (name == BlackSmithTroops.Ranger.ToString())
            {
                for (int i = 0; i < amount; i++)
                {
                    var troop = new Troop
                    {
                        Name = name,
                        StartedAt = GetTroopStartTime(troopsInProgress),
                        FinishedAt = GetTroopFinishTime(troopsInProgress, 1),
                        Level = 1,
                        Attack = 20,
                        Defense = 0,
                        Capacity = _rules.TroopCapacity(1),
                        ConsumingFood = false,
                        KingdomId = kingdomId
                    };
                    troopsInProgress.Add(troop);
                    newTroops.Add(troop);
                }

                return newTroops;
            }

            if (name == BlackSmithTroops.Scout.ToString())
            {
                for (int i = 0; i < amount; i++)
                {
                    var troop = new Troop
                    {
                        Name = name,
                        StartedAt = GetTroopStartTime(troopsInProgress),
                        FinishedAt = GetTroopFinishTime(troopsInProgress, 1),
                        Level = 1,
                        Attack = 0,
                        Defense = 1,
                        Capacity = _rules.TroopCapacity(1),
                        ConsumingFood = false,
                        KingdomId = kingdomId
                    };
                    troopsInProgress.Add(troop);
                    newTroops.Add(troop);
                }

                return newTroops;
            }

            for (int i = 0; i < amount; i++)
            {
                var troop = new Troop
                {
                    StartedAt = GetTroopStartTime(troopsInProgress),
                    FinishedAt = GetTroopFinishTime(troopsInProgress, 1),
                    Level = 1,
                    Attack = _rules.TroopAttack(1),
                    Defense = _rules.TroopDefense(1),
                    Capacity = _rules.TroopCapacity(1),
                    ConsumingFood = false,
                    KingdomId = kingdomId
                };
                //This call is required so that the next iteration of the loop has correct data to work with.
                troopsInProgress.Add(troop);
                newTroops.Add(troop);
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

        public KingdomTroopsDTO UpgradeTroops(int kingdomId, TroopUpgradeRequestDTO request)
        {
            var troopsInProgress = _applicationContext.Troops
                .Where(t => t.FinishedAt > _timeService.GetCurrentSeconds() && t.KingdomId == kingdomId)
                .OrderBy(t => t.FinishedAt)
                .ToList();
            var troopsToUpgrade = PerformUpgradeChecksAndReturnTroopsToUpgrade(kingdomId, request);

            foreach (var troop in troopsToUpgrade)
            {
                var level = troop.Level + 1;
                troop.ConsumingFood = false;
                troop.Level = level;
                //create a function for this
                troop.StartedAt = GetTroopStartTime(troopsInProgress);
                troop.FinishedAt = GetTroopFinishTime(troopsInProgress, level);
                troop.Attack = _rules.TroopAttack(level);
                troop.Defense = _rules.TroopDefense(level);
                troop.Capacity = _rules.TroopCapacity(level);
                troopsInProgress.Add(troop);
            }

            _applicationContext.SaveChanges();

            var responseDTO = new KingdomTroopsDTO
            {
                Troops = troopsToUpgrade
            };
            return responseDTO;
        }

        public TroopResponseDTO TrainSpecialTroops(int kingdomId, TroopRequestDTO request)
        {
            var troopsName = request.Name;

            if (troopsName == null || request.NumberOfTroops <= 0)
            {
                throw new TroopCreationException("Data not provided correctly");
            }

            var kingdom = _applicationContext.Kingdoms
                .Include(k => k.Buildings)
                .Include(k => k.Resources)
                .Include(k => k.Troops)
                .Include(k => k.BuildingUpgrades)
                .FirstOrDefault(k => k.KingdomId == kingdomId);


            if (troopsName == BlackSmithTroops.Ranger.ToString() || troopsName == BlackSmithTroops.Scout.ToString())
            {
                var result = CreatingTroopsFromBlacksmith(kingdom, request);
                return result;
            }

            throw new TroopCreationException("Special troop with this name does not exist");
        }

        private TroopResponseDTO CreatingTroopsFromBlacksmith(Kingdom kingdom, TroopRequestDTO request)
        {
            var specialTroop = request.Name!;

            if (kingdom.Buildings.FirstOrDefault(b => b.Type == BuildingType.Blacksmith) == null)
            {
                throw new TroopCreationException("You need an Blacksmith to be able to train troops.");
            }


            if (kingdom.BuildingUpgrades.FirstOrDefault(u => u.Name == specialTroop) == null)
            {
                throw new TroopCreationException(
                    "You need to buy an upgrade inside Blacksmith to be able to train the troops.");
            }

            var kingdomUpgrade = kingdom.BuildingUpgrades.First(u => u.Name == specialTroop);

            if (kingdomUpgrade.FinishedAt > _timeService.GetCurrentSeconds())
            {
                throw new TroopCreationException("Your upgrade is not finished yet");
            }

            var orderPrice = 0;

            if (specialTroop == BlackSmithTroops.Ranger.ToString())
            {
                orderPrice = _rules.CostSpecialTroopRanger(1) * request.NumberOfTroops;
            }

            if (specialTroop != BlackSmithTroops.Scout.ToString())
            {
                orderPrice = _rules.CostSpecialTroopScout(1) * request.NumberOfTroops;
            }

            var kingdomGold = kingdom!.Resources.FirstOrDefault(r => r.Type == ResourceType.Gold);
            var troopsInProgress = kingdom.Troops
                .Where(t => t.FinishedAt > _timeService.GetCurrentSeconds())
                .ToList();

            PerformTrainingChecks(kingdom, request.NumberOfTroops, kingdomGold.Amount, orderPrice);

            var newTroops = CreateNewTroops(kingdom.KingdomId, request.NumberOfTroops, troopsInProgress, request.Name);

            // create method for this code because it is redundant code  
            foreach (var troop in newTroops)
            {
                kingdom.Troops.Add(troop);
            }

            _applicationContext.SaveChanges();
            return new TroopResponseDTO
            {
                NewTroops = newTroops
            };
        }

        private List<Troop> PerformUpgradeChecksAndReturnTroopsToUpgrade(int kingdomId, TroopUpgradeRequestDTO request)
        {
            //Check that the troops we want to upgrade actually belong to the kingdom
            List<Troop> troopsToUpgrade = new List<Troop>();
            int upgradePrice = 0;
            foreach (var troopId in request.TroopIds)
            {
                var troop = _applicationContext.Troops.FirstOrDefault(t => t.TroopId == troopId);
                if (troop!.KingdomId != kingdomId)
                {
                    throw new TroopCreationException("Invalid Troop ID.");
                }

                upgradePrice += _rules.TroopPrice(troop.Level + 1);
                troopsToUpgrade.Add(troop);
            }

            troopsToUpgrade = troopsToUpgrade.OrderBy(t => t.Level).ToList();

            //Check that the best troop's level after upgrade isn't higher than the academy's
            if ((troopsToUpgrade[0].Level + 1) >
                _applicationContext.Buildings.FirstOrDefault(b =>
                    b.Type == BuildingType.Academy && b.KingdomId == kingdomId)!.Level)
            {
                throw new TroopCreationException("Upgrade not allowed, academy level too low.");
            }

            //Check the kingdom has enough money

            if (upgradePrice > _applicationContext.Resources
                    .FirstOrDefault(r => r.Type == ResourceType.Gold && r.KingdomId == kingdomId)!.Amount)
            {
                throw new TroopCreationException("Not enough gold to upgrade.");
            }

            return troopsToUpgrade;
        }

        private long GetTroopStartTime(List<Troop> troopsInProgress)
        {
            if (troopsInProgress.Count != 0)
            {
                return troopsInProgress.Last().FinishedAt;
            }

            return _timeService.GetCurrentSeconds();
        }

        private long GetTroopFinishTime(List<Troop> troopsInProgress, int level)
        {
            if (troopsInProgress.Count != 0)
            {
                return troopsInProgress.Last().FinishedAt + _rules.TroopBuildingTime(level);
            }

            return _timeService.GetCurrentSeconds() + _rules.TroopBuildingTime(level);
        }
    }
}