using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class KingdomBuildingsDto
    {
        public ICollection<Building> Buildings { get; set; }
    }
}