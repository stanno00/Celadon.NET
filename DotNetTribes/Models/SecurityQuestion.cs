using System.ComponentModel.DataAnnotations.Schema;
using DotNetTribes.Enums;

namespace DotNetTribes.Models
{
    public class SecurityQuestion
    {
        public int SecurityQuestionId { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public SecurityQuestionType TheQuestion { get; set; }
        public string Answer { get; set; }
        public User? User { get; set; }
    }
}