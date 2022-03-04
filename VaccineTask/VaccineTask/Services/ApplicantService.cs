using System.Text.RegularExpressions;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationContext _applicationContext;

        public ApplicantService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Applicant AddApplicant(ApplicantDto applicantDto)
        {
            var regex = new Regex("\\d{3}-\\d{3}-\\d{3}");
            if (!regex.IsMatch(applicantDto.SocialSecurityNumber))
            {
                return null;
            }

            if (applicantDto.Gender.ToLower() != "male" && applicantDto.Name.ToLower() != "female" && applicantDto.Name.ToLower() != "other")
            {
                return null;
            }

            var applicant = new Applicant()
            {
                Name = applicantDto.Name,
                Gender = applicantDto.Gender,
                SocialSecurityNumber = applicantDto.SocialSecurityNumber,
                DateOfBirth = applicantDto.DateOfBirth

            };

            _applicationContext.Add(applicant);
            _applicationContext.SaveChanges();
            return applicant;
        }

        public Applicant GetApplicant(int applicantId)
        {
            throw new System.NotImplementedException();
        }

        public Applicant UpdateApplicant(int applicantId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveApplicant(int applicantId)
        {
            throw new System.NotImplementedException();
        }
    }
}