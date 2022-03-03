using EntityFrameworkMatReview.DTOs;
using EntityFrameworkMatReview.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkMatReview.Services
{
    public class StudentService : IStudentService
    {

        private readonly ApplicationContext applicationContext;

        public StudentService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Student AddStudent(CreateStudentDTO createStudentDTO)
        {
            var student = new Student()
            {
                Name = createStudentDTO.Name,
                Age = createStudentDTO.Age
            };
            applicationContext.Add(student);
            applicationContext.SaveChanges();
            return student;

        }

        public Student GetById(int id)
        {
            var student = applicationContext.Students
                .Include(s => s.Pet)
                .Include(s => s.Lockers)
                .Include(s => s.StudentTeacher)
                    .ThenInclude(st => st.Teacher)
                .SingleOrDefault(s => s.StudentId == id);
            return student;
        }
    }
}
