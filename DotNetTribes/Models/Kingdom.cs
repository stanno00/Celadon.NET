using System.Collections.Generic;
using DotNetTribes.DTOs;

namespace DotNetTribes.Models
{
    public class Kingdom
    {
        public int KingdomId { get; set; }
        public string Name { get; set; }

        public User? User { get; set; }
        public ICollection<Building> Buildings { get; set; }
        public ICollection<Troop> Troops { get; set; }
        public ICollection<Resource> Resources { get; set; }
        
        public ICollection<UnfinishedTroop> TroopsWorkedOn { get; set; }
    }
}