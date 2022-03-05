using System;

namespace VaxAPI.DTOs
{
    public class ApplicantDTO
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SocialSecurityNo { get; set; }
        public string Gender { get; set; }
    }
}