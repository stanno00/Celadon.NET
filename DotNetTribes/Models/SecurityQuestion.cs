using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetTribes.Models
{
    public class SecurityQuestion
    {
        public long SecurityQuestionId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public SecurityQuestion TheQuestion { get; set; }
        public string Answer { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}