using System.Collections.Generic;
using DotNetTribes.DTOs.Troops;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
   public interface ITroopService
    {
        public TroopResponseDTO TrainTroops(int kingdomId, TroopRequestDTO request);

        public KingdomTroopsDTO GetKingdomTroops(int kingdomId);
        public void UpdateTroops(int kingdomId);

        public KingdomTroopsDTO UpgradeTroops(int kingdomId, TroopUpgradeRequestDTO request);
        List<Troop> GetReadyTroops(int kingdomId);
    }
}