using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IVaccineService
    {
        Vaccine AddVaccine(VaccineDto vaccineDto);
        Vaccine UpdateVaccine(VaccineDto vaccineDto, int vaccineId);
        Vaccine GetVaccine(int vaccineId);
        Vaccine DeleteVaccine(int vaccineId);
    }
}