using System;

namespace VaccineTask.DTOs
{
    public class ApplicantDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
    }
}