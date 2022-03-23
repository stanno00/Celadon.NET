using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class KingdomBuildingsDto
    {
        public ICollection<BuildingResponseDTO> Buildings { get; set; }
    }
}