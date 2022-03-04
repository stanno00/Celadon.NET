using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VaxAPI.DTOs;
using VaxAPI.Models;

namespace VaxAPI.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly ApplicationContext _ac;

        public HospitalService(ApplicationContext ac)
        {
            _ac = ac;
        }
        
        public bool AddNewHospital(HospitalDTO incoming)
        {
            try
            {
                _ac.Add(new Hospital
                {
                    Name = incoming.Name,
                    Budget = incoming.Budget
                });
                _ac.SaveChanges();
                return true;
            }
            catch (Exception ignored)
            {
                return false;
            }
        }
        
        public string GetHospitalName(long hospitalId)
        {
            return _ac.Hospitals.FirstOrDefault(h => h.HospitalId== hospitalId).Name;
        }
        
        public bool ChangeHospitalName(HospitalDTO incoming, long hospitalId)
        {
            try
            {
                Hospital hospital = _ac.Hospitals.FirstOrDefault(h => h.HospitalId == hospitalId);
                hospital.Name = incoming.Name;
                _ac.Update(hospital);
                _ac.SaveChanges();
                return true;
            }
            catch (Exception ignored)
            {
                return false;
            }
        }
        
        public int OrderVaccines(VaccineOrder order)
        {
            Hospital orderedFor = _ac.Hospitals
                .Include(h => h.HospitalStock)
                .FirstOrDefault(h => h.HospitalId == order.HospitalId);
            
            if(orderedFor == null)
            {
                return 1;
            }
            
            if(orderedFor.Budget < order.TotalPrice)
            {
                return 2;
            }

            if (orderedFor.HospitalStock.FirstOrDefault(stock =>
                    stock.HospitalId == order.HospitalId && stock.VaccineId == order.VaccineId) == null)
            {
                orderedFor.HospitalStock.Add(new HospitalStock(orderedFor.HospitalId, order.VaccineId));
                _ac.SaveChanges();
            }
            
            var stock = orderedFor.HospitalStock.FirstOrDefault(stock =>
                stock.HospitalId == order.HospitalId && stock.VaccineId == order.VaccineId);

            stock.StockAmount += order.OrderAmount;
            orderedFor.Budget -= order.TotalPrice;
            _ac.SaveChanges();
            return 3;
        }

        public Dictionary<string, int> CheckVaccinesInHospital(long hospitalId)
        {
            Hospital inspected = _ac.Hospitals
                .Include(h => h.HospitalStock)
                .ThenInclude(s => s.Vaccine)
                .FirstOrDefault(h => h.HospitalId == hospitalId);
            
            if (inspected == null)
            {
                return null;
            }

            var stock = inspected.HospitalStock.ToDictionary(key => key.Vaccine.Name, value => value.StockAmount);
            return stock;
        }


    }
}