using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IKingdomService
    {
        KingdomDto KingdomInfo(int kingdomId);

        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request);
    }
}