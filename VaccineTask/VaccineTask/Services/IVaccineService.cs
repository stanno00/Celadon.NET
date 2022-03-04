using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IVaccineService
    {
        Vaccine AddVaccine(VaccineDto vaccineDto);
    }
}