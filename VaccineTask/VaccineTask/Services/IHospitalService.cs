using System.Collections.Generic;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public interface IHospitalService
    {
        List<Hospital> Hospitals();
        Hospital AddHospital(HospitalDto hospitalDto);
        Hospital GetHospital(int hospitalId);
        Hospital UpdateHospital(int hospitalId, HospitalDto hospitalDto);
        Hospital DeleteHospital(int hospitalId);

        Hospital VaccineOrder(VaccineOrderDto vaccineOrderDto);
    }
}