using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityFrameworkMatReview.Models
{
    public class Locker
    {
        //[JsonIgnore]
        public int LockerId { get; set; }
        public string Size { get; set; }
        public int? StudentId { get; set; }
    }
}
