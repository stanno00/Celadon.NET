using IntroDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroDotNet.Services
{
    public interface IStudentService
    {
        Student GetByName(string name);
        List<Student> FindAll();
        Student AddStudent(Student student);
        int GetSumOfAge();
    }
}
