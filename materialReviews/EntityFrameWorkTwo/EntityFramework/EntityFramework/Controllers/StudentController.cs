using EntityFramework.Models;
using EntityFramework.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Controllers
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

        [HttpGet("{studentId}")]
        public ActionResult<Student> GetById([FromRoute] int studentId)
        {
            var student = studentService.GetById(studentId);
            return student;
        }
    }
}
