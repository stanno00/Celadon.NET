using Newtonsoft.Json;

namespace DotNetTribes.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}