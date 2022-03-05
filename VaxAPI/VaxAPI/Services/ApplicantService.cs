using System;
using VaxAPI.DTOs;
using VaxAPI.Models;

namespace VaxAPI.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationContext _ac;

        public ApplicantService(ApplicationContext ac)
        {
            _ac = ac;
        }

        public bool AddNewApplicant(ApplicantDTO incoming)
        {
            try
            {
                _ac.Applicants.Add(new Applicant
                {
                    DateOfBirth = DateTime.Parse(incoming.DateOfBirth),
                    Name = incoming.Name,
                    Gender = incoming.Gender,
                    SocialSecurityNo = incoming.SocialSecurityNo
                });
                _ac.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}