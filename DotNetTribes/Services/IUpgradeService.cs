using DotNetTribes.DTOs;
using DotNetTribes.Enums;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public interface IUpgradeService
    {
        BuildingsUpgradesResponseDto AddUpgrade(int kingdomId, BuildingsUpgradesRequestDto upgrade);
        public UniversityUpgrade BuyUniversityUpgrade(int kingdomId, UpgradeType upgradeType);
        public void ApplyUpgradesWhenFinished();
    }
}