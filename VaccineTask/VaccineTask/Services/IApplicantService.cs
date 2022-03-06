using System.Collections.Generic;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IApplicantService
    {
        public List<Applicant> Applicants();
        Applicant AddApplicant(ApplicantDto applicantDto);
        Applicant GetApplicant(int applicantId);
        Applicant UpdateApplicant(int applicantId, ApplicantDto applicantDto);
        Applicant RemoveApplicant(int applicantId);
    }
}