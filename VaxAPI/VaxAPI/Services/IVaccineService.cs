using VaxAPI.DTOs;
using VaxAPI.Models;

namespace VaxAPI.Services
{
    public interface IVaccineService
    {
        public bool CreateNewVaccine(VaccineDTO incoming);
        public Vaccine GetVaccineById(long vaccineId);
        public bool DeleteVaccine(long vaccineId);
        public bool UpdateVaccineName(VaccineDTO incoming, long vaccineId);
        public long? AddNewVaccineOrder(VaccineOrderDTO incoming);
        public VaccineOrder GetOrderById(long vaccineOrderId);
    }
}