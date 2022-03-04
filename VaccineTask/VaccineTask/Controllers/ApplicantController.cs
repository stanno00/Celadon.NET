using Microsoft.AspNetCore.Mvc;
using VaccineTask.DTOs;
using VaccineTask.Models;
using VaccineTask.Services;

namespace VaccineTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController
    {

        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [HttpPost]
        public ActionResult<Applicant> AddApplicant([FromBody] ApplicantDto applicantDto)
        {
            var applicant = _applicantService.AddApplicant(applicantDto);
            if (applicant == null)
            {
                return new BadRequestObjectResult("Wrong input!");
            }

            return new CreatedResult("", applicant);
        }

    }
}