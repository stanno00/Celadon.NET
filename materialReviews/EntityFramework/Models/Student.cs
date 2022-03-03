using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkMatReview.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int? PetId { get; set; }
        public Pet Pet { get; set; }

        public List<Locker> Lockers { get; set; }

        public List<StudentTeacher> StudentTeacher { get; set; }
    }
}
