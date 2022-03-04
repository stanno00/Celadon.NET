using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            var hospital = _applicationContext.Hospitals
                .Include(h => h.VaccineOrders)
                .FirstOrDefault(h => h.HospitalId == hospitalId);
            return hospital;
        }

        public Hospital UpdateHospital(int hospitalId, HospitalDto hospitalDto)
        {
            throw new System.NotImplementedException();
        }

        public Hospital DeleteHospital(int hospitalId)
        {
            throw new System.NotImplementedException();
        }

        public Hospital VaccineOrder(VaccineOrderDto vaccineOrderDto)
        {
            var hospital = _applicationContext.Hospitals.FirstOrDefault(h => h.Name == vaccineOrderDto.HospitalName);
            var vaccine = _applicationContext.Vaccines.FirstOrDefault(v => v.Name == vaccineOrderDto.VaccineName);
            if (hospital == null || vaccine == null)
            {
                return null;
            }

            var vaccineOrder = new VaccineOrder()
            {
                HospitalName = vaccineOrderDto.HospitalName,
                VaccineName = vaccineOrderDto.VaccineName,
                NumberOfVaccinesBeingOrdered = vaccineOrderDto.NumberOfVaccinesBeingOrdered,
                TotalPriceOfVaccines = vaccine.Price * vaccineOrderDto.NumberOfVaccinesBeingOrdered
            };
            
            if (hospital.Budget < vaccineOrder.TotalPriceOfVaccines)
            {
                return null;
            }
            
            hospital.Budget -= vaccineOrder.TotalPriceOfVaccines;
            hospital.VaccineOrders.Add(vaccineOrder);

            _applicationContext.VaccineOrders.Add(vaccineOrder);
            _applicationContext.Hospitals.Update(hospital);
            _applicationContext.SaveChanges();
            return hospital;
        }
    }
}