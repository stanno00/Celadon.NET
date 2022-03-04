using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VaxAPI.DTOs;
using VaxAPI.Models;

namespace VaxAPI.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly ApplicationContext _ac;

        public VaccineService(ApplicationContext ac)
        {
            _ac = ac;
        }
        
        public bool CreateNewVaccine(VaccineDTO incoming)
        {
            //TODO add uniqueness check
            try
            {
                _ac.Add(new Vaccine
                {
                    Name = incoming.Name,
                    Price = incoming.Price
                });
                _ac.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Vaccine GetVaccineById(long vaccineId)
        {
            try
            {
                return _ac.Vaccines.FirstOrDefault(v => v.VaccineId == vaccineId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteVaccine(long vaccineId)
        {
            try
            {
                Vaccine toBeDeleted = _ac.Vaccines.FirstOrDefault(v => v.VaccineId == vaccineId);
                //null check not required - if the vaccine does not exist, the app throws an exception which is caught.
                _ac.Vaccines.Remove(toBeDeleted);
                _ac.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public bool UpdateVaccineName(VaccineDTO incoming ,long vaccineId)
        {
            //TODO add uniqueness check
            try
            {
                Vaccine updated = _ac.Vaccines.FirstOrDefault(v => v.VaccineId == vaccineId);
                updated.Name = incoming.Name;
                _ac.Update(updated);
                _ac.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public long? AddNewVaccineOrder(VaccineOrderDTO incoming)
        {
            try
            {
                VaccineOrder order = new  VaccineOrder
                {
                    HospitalId = incoming.HospitalId,
                    VaccineId = incoming.VaccineId,
                    OrderAmount = incoming.OrderAmount

                };
                _ac.Add(order);
                _ac.SaveChanges();
                return order.VaccineOrderId;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public VaccineOrder GetOrderById(long vaccineOrderId)
        {
            try
            {
                var order = _ac.VaccineOrders
                    .Include(vo => vo.Vaccine)
                    .Include(vo => vo.Hospital)
                    .Single(vo => vo.VaccineOrderId == vaccineOrderId);
                return order;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}