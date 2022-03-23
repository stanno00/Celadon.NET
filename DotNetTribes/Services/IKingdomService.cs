using System.Collections.Generic;
using DotNetTribes.DTOs;

namespace DotNetTribes.Services
{
    public interface IKingdomService
    {
        KingdomDto KingdomInfo(int kingdomId);
        object NearestKingdoms(int minutes,int kingdomId);
        public List<BuildingResponseDTO> GetExistingBuildings(int kingdomId);
    }
}