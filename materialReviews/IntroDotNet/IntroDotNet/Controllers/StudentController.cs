using IntroDotNet.DTOs;
using IntroDotNet.Models;
using IntroDotNet.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroDotNet.Controllers
{
    [ApiController] // RestController
    public class StudentController
    {

        private readonly IStudentService studentService;    // readonly == final

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet("/students")]
        public List<Student> ShowStudent()
        {
            return studentService.FindAll();
        }

        // PathVariable
        [HttpGet("/student/{name}")]
        public Student GetStudentByNamePath([FromRoute] string name)
        {
            var student = studentService.GetByName(name);
            return student;
        }

        // QueryParam /student?name=asdasd
        [HttpGet("/student")]
        public Student GetStudentByNameQuery([FromQuery] string name)
        {
            var student = studentService.GetByName(name);
            return student;
        }

        [HttpPost("/student")]
        public ActionResult<Student> CreateStudent([FromBody] CreateStudentRequestDTO studentDto)
        {
            if(studentDto == null)
            {
                return new BadRequestResult(); // return status code 400
            }

            var student = new Student()
            {
                Name = studentDto.Name,
                Age = studentDto.Age
            };

            var createdStudent = studentService.AddStudent(student);


            return new CreatedResult("", createdStudent);
        }


    }
}
