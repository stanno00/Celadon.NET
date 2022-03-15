using DotNetTribes.DTOs;

namespace DotNetTribes.Service
{
    public interface IKingdomService
    {
        KingdomDto KingdomInfo(int kingdomId);

        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request);
    }
}