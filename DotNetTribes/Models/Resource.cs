using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace DotNetTribes.Models

{
    public class Resource
    {
        public int ResourceId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}