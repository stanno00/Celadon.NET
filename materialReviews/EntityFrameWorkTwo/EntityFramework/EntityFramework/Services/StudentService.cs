using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationContext applicationContext;

        public StudentService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public Student GetById(int id)
        {
            var student = applicationContext.Students
                .Include(s => s.Pet)
                .Include(s => s.Lockers)
                .Include(s => s.StudentTeachers)
                .ThenInclude(s => s.Teacher)
                .Single(s => s.StudentId == id);

            return student;
        }
    }
}
