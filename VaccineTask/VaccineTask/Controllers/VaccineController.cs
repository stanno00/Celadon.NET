using Microsoft.AspNetCore.Mvc;
using VaccineTask.DTOs;
using VaccineTask.Models;
using VaccineTask.Services;

namespace VaccineTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VaccineController
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        [HttpPost]
        public ActionResult<Vaccine> AddVaccine([FromBody] VaccineDto vaccineDto)
        {
            var vaccine = _vaccineService.AddVaccine(vaccineDto);
            if (vaccine == null)
            {
                return new BadRequestObjectResult("Vaccine already exists!");
            }
            return new CreatedResult("", vaccine);
        }
    }
}