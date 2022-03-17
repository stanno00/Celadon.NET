using System.ComponentModel.DataAnnotations.Schema;



namespace DotNetTribes.Models

{
    public class Resource
    {
        public int ResourceId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public int Generation { get; set; }
        public long UpdatedAt { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }
    }
}