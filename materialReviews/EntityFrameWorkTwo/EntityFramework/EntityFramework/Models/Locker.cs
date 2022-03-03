using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Models
{
    public class Locker
    {
        public int LockerId { get; set; }
        public int Size { get; set; }
        public int StudentId { get; set; }
    }
}
