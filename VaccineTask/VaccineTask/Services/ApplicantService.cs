using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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

        public List<Applicant> Applicants()
        {
            return _applicationContext.Applicants.AsEnumerable().ToList();
        }

        public Applicant AddApplicant(ApplicantDto applicantDto)
        {
            if (!CheckIfDtoIsCorrect(applicantDto)) return null;
            
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
            var applicant = _applicationContext.Applicants
                .Include(a => a.Applications)
                .SingleOrDefault(a => a.ApplicantId == applicantId);
            return applicant;
        }

        public Applicant UpdateApplicant(int applicantId, ApplicantDto applicantDto)
        {
            var applicant = GetApplicant(applicantId);
            
            if (applicant == null) return null;
            if (!CheckIfDtoIsCorrect(applicantDto)) { return null; }
            
            applicant.Gender = applicantDto.Gender;
            applicant.Name = applicantDto.Name;
            applicant.DateOfBirth = applicantDto.DateOfBirth;
            applicant.SocialSecurityNumber = applicantDto.SocialSecurityNumber;

            _applicationContext.Applicants.Update(applicant);
            _applicationContext.SaveChanges();
            return applicant;

        }

        public Applicant RemoveApplicant(int applicantId)
        {
            var applicant = GetApplicant(applicantId);
            if (applicant == null) return null;
            
            _applicationContext.Applicants.Remove(applicant);
            _applicationContext.SaveChanges();
            return applicant;

        }

        private bool CheckIfDtoIsCorrect(ApplicantDto applicantDto)
        {
            var regex = new Regex("\\d{3}-\\d{3}-\\d{3}");
            if (!regex.IsMatch(applicantDto.SocialSecurityNumber)) { return false; }

            var applicants = Applicants();
            if (applicants.Any(applicant => applicant.SocialSecurityNumber == applicantDto.SocialSecurityNumber))
            {
                return false;
            }

            if (applicantDto.Gender.ToLower() != "male" && applicantDto.Name.ToLower() != "female" && applicantDto.Name.ToLower() != "other")
            {
                return false;
            }

            return true;
        }

        public Application PostApplication(int applicantId, ApplicationDto applicationDto)
        {
            var hospital = _applicationContext.Hospitals.FirstOrDefault(h => h.Name == applicationDto.HospitalName);
            var vaccine = _applicationContext.Vaccines.FirstOrDefault(v => v.Name == applicationDto.VaccineName);

            if (hospital == null || vaccine == null)
            {
                return null;
            }
            
            var application = new Application
            {
                ApplicantName = applicationDto.ApplicantName,
                HospitalName = applicationDto.HospitalName,
                VaccineName = applicationDto.VaccineName,
                DateOfApplication = DateTime.Now
            };

            var applicant = GetApplicant(applicantId);
            
            applicant.Applications.Add(application);
            _applicationContext.Applicants.Update(applicant);
            _applicationContext.SaveChanges();
            
            return application;
        }
    }
}