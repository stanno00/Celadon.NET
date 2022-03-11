namespace DotNetTribes.Models
using Newtonsoft.Json;

{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}