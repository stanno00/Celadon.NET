using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class KingdomDto
    {
        public string KingdomName { get; set; }
        public string Username { get; set; }
        public List<BuildingResponseDTO> Buildings { get; set; }
        public ICollection<Troop> Troops { get; set; }
        public ICollection<ResourceDto> Resources { get; set; }
    }
}