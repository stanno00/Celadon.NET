using System.Collections.Generic;
using System.Linq;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly ApplicationContext _applicationContext;

        public HospitalService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<Hospital> Hospitals()
        {
            return _applicationContext.Hospitals.AsEnumerable().ToList();
        }

        public Hospital AddHospital(HospitalDto hospitalDto)
        {
            var hospital = new Hospital()
            {
                Name = hospitalDto.Name,
                Budget = hospitalDto.Budget
            };

            _applicationContext.Hospitals.Add(hospital);
            _applicationContext.SaveChanges();
            return hospital;
        }

        public Hospital GetHospital(int hospitalId)
        {
            throw new System.NotImplementedException();
        }

        public Hospital UpdateHospital(int hospitalId, HospitalDto hospitalDto)
        {
            throw new System.NotImplementedException();
        }

        public Hospital DeleteHospital(int hospitalId)
        {
            throw new System.NotImplementedException();
        }
    }
}