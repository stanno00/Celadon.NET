using System.Collections.Generic;

namespace DotNetTribes.Models
{
    public class Kingdom
    {
        public int KingdomId { get; set; }
        public string Name { get; set; }
        public List<Resource> Resources { get; set; }
    }
}