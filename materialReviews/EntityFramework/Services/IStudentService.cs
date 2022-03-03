using EntityFrameworkMatReview.DTOs;
using EntityFrameworkMatReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkMatReview
{
    public interface IStudentService
    {
        Student AddStudent(CreateStudentDTO createStudentDTO);
        Student GetById(int id);
    }
}
