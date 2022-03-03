using EntityFrameworkMatReview.DTOs;
using EntityFrameworkMatReview.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkMatReview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        // /student
        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] CreateStudentDTO createStudentDTO)
        {
            var student = studentService.AddStudent(createStudentDTO);
            return new CreatedResult("", student);
        }
        // /student/1

        [HttpGet("{studentId}")]
        public ActionResult<Student> GetStudentById([FromRoute] int studentId)
        {
            var student = studentService.GetById(studentId);
            return student;
        }
    }
}
