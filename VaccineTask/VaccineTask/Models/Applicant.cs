using System;
using System.Collections.Generic;

namespace VaccineTask.Models
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public DateTime Type { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }

        public List<Application> Applications { get; set; }
    }
}