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

        [HttpGet("{hospitalId}")]
        public ActionResult<Hospital> GetHospital([FromRoute] int hospitalId)
        {
            var hospital = _hospitalService.GetHospital(hospitalId);
            if (hospital == null)
            {
                return new NotFoundObjectResult("Hospital not found");
            }

            return new OkObjectResult(hospital);
        }

        [HttpPost]
        public ActionResult<Hospital> PostHospital([FromBody] HospitalDto hospitalDto)
        {
            var hospital = _hospitalService.AddHospital(hospitalDto);
            return new CreatedResult("", hospital);
        }

        [HttpPut]
        public ActionResult<Hospital> OrderVaccines([FromBody] VaccineOrderDto vaccineOrderDto)
        {
            var hospital = _hospitalService.VaccineOrder(vaccineOrderDto);
            if (hospital == null)
            {
                return new BadRequestObjectResult("Bad request");
            }

            return new OkObjectResult(hospital);
        }

        [HttpPut("{hospitalId}")]
        public ActionResult<Hospital> UpdateHospital([FromBody] HospitalDto hospitalDto, [FromRoute] int hospitalId)
        {
            var hospital = _hospitalService.UpdateHospital(hospitalId, hospitalDto);
            if (hospital == null)
            {
                return new NotFoundObjectResult("Hospital does not exist!");
            }

            return new OkObjectResult(hospital);
        }
    }
}