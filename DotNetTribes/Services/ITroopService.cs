using DotNetTribes.DTOs.Troops;

namespace DotNetTribes.Services
{
   public interface ITroopService
    {
        public TroopResponseDTO TrainTroops(int kingdomId, TroopRequestDTO request);

        public KingdomTroopsDTO GetKingdomTroops(int kingdomId);
        public void UpdateTroops(int kingdomId);

        public KingdomTroopsDTO UpgradeTroops(int kingdomId, TroopUpgradeRequestDTO request);
        TroopResponseDTO TrainSpecialTroops(int kingdomId,TroopRequestDTO request);
    }
}