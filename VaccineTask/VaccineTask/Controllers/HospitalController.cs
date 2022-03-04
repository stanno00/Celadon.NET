using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VaccineTask.DTOs;
using VaccineTask.Models;
using VaccineTask.Services;

namespace VaccineTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalController
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpGet]
        public ActionResult<List<Hospital>> Hospitals()
        {
            var hospitals = _hospitalService.Hospitals();
            return new OkObjectResult(hospitals);
        }

        [HttpPost]
        public ActionResult<Hospital> GetHospital([FromBody] HospitalDto hospitalDto)
        {
            var hospital = _hospitalService.AddHospital(hospitalDto);
            return new CreatedResult("", hospital);
        }
    }
}