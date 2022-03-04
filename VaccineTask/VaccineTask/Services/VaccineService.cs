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
            var vaccine = new Vaccine()
            {
                Name = vaccineDto.Name,
                Price = vaccineDto.Price
            };

            _applicationContext.Vaccines.Add(vaccine);
            _applicationContext.SaveChanges();
            return vaccine;
        }
    }
}