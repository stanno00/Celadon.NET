using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IApplicantService
    {
        Applicant AddApplicant(ApplicantDto applicantDto);
        Applicant GetApplicant(int applicantId);
        Applicant UpdateApplicant(int applicantId);
        void RemoveApplicant(int applicantId);
        
    }
}