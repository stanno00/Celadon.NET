using System.Collections.Generic;

namespace DotNetTribes.DTOs
{
    public class KingdomBuildingsDto
    {
        public ICollection<BuildingResponseDTO> Buildings { get; set; }
    }
}