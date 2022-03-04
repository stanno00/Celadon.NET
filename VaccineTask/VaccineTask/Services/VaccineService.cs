using System.Collections.Generic;
using System.Linq;
using VaccineTask.DTOs;
using VaccineTask.Models;

namespace VaccineTask.Services
{
    public class VaccineService : IVaccineService
    {

        private readonly ApplicationContext _applicationContext;

        public VaccineService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Vaccine AddVaccine(VaccineDto vaccineDto)
        {
            if (_applicationContext.Vaccines.Any(x => x.Name == vaccineDto.Name))
            {
                return null;
            }
            var vaccine = new Vaccine()
            {
                Name = vaccineDto.Name,
                Price = vaccineDto.Price
            };

            _applicationContext.Vaccines.Add(vaccine);
            _applicationContext.SaveChanges();
            return vaccine;
        }

        public Vaccine GetVaccine(int vaccineId)
        {
            return _applicationContext.Vaccines.FirstOrDefault(v => v.VaccineId == vaccineId);
        }

        public Vaccine UpdateVaccine(VaccineDto vaccineDto, int vaccineId)
        {
            var vaccine = GetVaccine(vaccineId);
            if (vaccine == null)
            {
                return null;
            }

            vaccine.Name = vaccineDto.Name;
            _applicationContext.Vaccines.Update(vaccine);
            _applicationContext.SaveChanges();
            return vaccine;
        }

        public Vaccine DeleteVaccine(int vaccineId)
        {
            var vaccine = GetVaccine(vaccineId);
            if (vaccine == null)
            {
                return null;
            }
            
            _applicationContext.Vaccines.Remove(vaccine);
            _applicationContext.SaveChanges();
            return vaccine;
        }
    }
}