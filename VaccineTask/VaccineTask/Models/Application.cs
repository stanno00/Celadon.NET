using System;

namespace VaccineTask.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string HospitalName { get; set; }
        public DateTime DateOfApplication { get; set; }
        public string VaccineName { get; set; }
        public DateTime DateOfVaccineApplication { get; set; }

        public int HospitalId { get; set; }

        public int ApplicantId { get; set; }
    }
}