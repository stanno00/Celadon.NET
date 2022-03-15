using System.Dynamic;

namespace DotNetTribes.Models

{
    public class Resource
    {
        //Do we actually need a resource ID? Can the PK be, say, kingdom or even the type?
        public int ResourceId { get; set; }
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}