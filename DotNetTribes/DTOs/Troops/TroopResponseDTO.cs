using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs.Troops
{
    public class TroopResponseDTO
    {
        public List<UnfinishedTroop> NewTroops { get; set; }
    }
}