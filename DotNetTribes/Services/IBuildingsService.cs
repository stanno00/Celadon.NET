using DotNetTribes.DTOs;
using DotNetTribes.Models;

namespace DotNetTribes.Services
{
    public interface IBuildingsService
    {
        public BuildingResponseDTO CreateNewBuilding(int kingdomId, BuildingRequestDTO request);
        
    }
}