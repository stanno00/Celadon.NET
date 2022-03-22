using System.Collections;
using System.Collections.Generic;
using DotNetTribes.Models;

namespace DotNetTribes.DTOs
{
    public class KingdomDto
    {
        public string KingdomName { get; set; }
        public string Username { get; set; }
        public BuildingsDTO Buildings { get; set; }
        public ICollection<Troop> Troops { get; set; }
        public ResourcesDto Resources { get; set; }
    }
}