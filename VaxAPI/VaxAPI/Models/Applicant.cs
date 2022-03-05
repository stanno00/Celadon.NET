using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaxAPI.Models
{
    public class Applicant
    {
        public long ApplicantId { get; set; }
        public string Name { get; set; }
        [Column(TypeName="date")]
        public DateTime DateOfBirth { get; set; }
        public string SocialSecurityNo { get; set; }
        private string _gender;
        public string Gender
        {
            get => _gender;
            set
            {
                if (value == "Male" || value == "male" || value.ToLower() == "m")
                {
                    _gender = "Male";
                }
                else if (value == "Female" || value == "female" || value.ToLower() == "f")
                {
                    _gender = "Female";
                }
                else
                {
                    _gender = "Other";
                }
            }
        }
    }
}