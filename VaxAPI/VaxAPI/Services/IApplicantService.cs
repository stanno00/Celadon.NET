using VaxAPI.DTOs;

namespace VaxAPI.Services
{
    public interface IApplicantService
    {
        public bool AddNewApplicant(ApplicantDTO incoming);
    }
}