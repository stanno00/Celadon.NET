using System;

namespace VaxAPI.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        private long ApplicantId { get; set; }
        private Applicant Applicant { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantSocialSecurity{ get; set; }
        private Hospital Hospital { get; set; }
        public string HospitalName { get; set; }
        public DateTime ApplicationDate { get; set; }
        private Vaccine Vaccine { get; set; }
        public string VaccineName{ get; set; }
        public DateTime VaccineApplicationDate { get; set; }
        
    }
}