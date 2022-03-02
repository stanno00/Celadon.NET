using IntroDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroDotNet.Services
{
    public class StudentService : IStudentService
    {
        // Repository placeholder
        private List<Student> students;

        public StudentService()
        {
            students = new List<Student>()
            {
                new Student("Josh", 40, 4.5),
                new Student("Anna", 22, 4.6),
                new Student("Marta", 26, 3.5)
            };
        }

        public Student AddStudent(Student student)
        {
            students.Add(student);
            return student;
        }

        public List<Student> FindAll()
        {
            return students;
        }

        public Student GetByName(string name)
        {
            var student = students.FirstOrDefault(s => s.Name == name);
            return student;
        }

        public int GetSumOfAge()
        {
            int sumOfAge = students.Select(s => s.Age).Sum();
            return sumOfAge;
        }
    }
}
