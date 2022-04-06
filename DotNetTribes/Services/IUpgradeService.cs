using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IUpgradeService
    {
        BuildingsUpgradesResponseDto AddUpgrade(int kingdomId, BuildingsUpgradesRequestDto upgrade);
    }
}