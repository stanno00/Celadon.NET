using System.Collections.Generic;
using VaxAPI.DTOs;
using VaxAPI.Models;

namespace VaxAPI.Services
{
    public interface IHospitalService
    {
        public bool AddNewHospital(HospitalDTO incoming);
        public string GetHospitalName(long hospitalId);

        public bool ChangeHospitalName(HospitalDTO incoming, long hospitalId);
        public int OrderVaccines(VaccineOrder order);
        public Dictionary<string, int> CheckVaccinesInHospital(long hospitalId);
    }
}