using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IApplicationService
    {
        Application PostApplication(int applicantId, ApplicationDto applicationDto);
    }
}