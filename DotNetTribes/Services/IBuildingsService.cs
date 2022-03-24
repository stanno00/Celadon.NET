using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IBuildingsService
    {
        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request);

        BuildingResponseDTO UpgradeBuilding(int kingdomId, int buildingId);
    }
}