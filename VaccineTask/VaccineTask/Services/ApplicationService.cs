using System;
using System.Linq;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IApplicantService _applicantService;

        public ApplicationService(ApplicationContext applicationContext, IApplicantService applicantService)
        {
            _applicationContext = applicationContext;
            _applicantService = applicantService;
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

            var applicant = _applicantService.GetApplicant(applicantId);
            
            applicant.Applications.Add(application);
            _applicationContext.Applicants.Update(applicant);
            _applicationContext.SaveChanges();
            
            return application;
        }
    }
}