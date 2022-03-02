using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroDotNet.Models
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double AverageGrade { get; set; }

        public Student(string name, int age, double averageGrade)
        {
            Name = name;
            Age = age;
            AverageGrade = averageGrade;
        }

        public Student()
        {
        }
    }
}
