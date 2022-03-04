using System;

namespace VaxAPI.Models
{
    public class Applicant
    {
        public long ApplicantId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SocialSecurityNo { get; set; }

        // the same could also be achieved by using an enum
        public string Gender
        {
            get { return Gender; }
            set
            {
                if (value == "Male" || value == "male")
                {
                    Gender = "Male";
                }
                else if (value == "Female" || value == "female")
                {
                    Gender = "Female";
                }
                else
                {
                    Gender = "Other";
                }
            }
        }
    }
}