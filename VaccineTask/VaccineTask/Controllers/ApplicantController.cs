using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult<List<Applicant>> Applicants()
        {
            var applicants = _applicantService.Applicants();
            return new OkObjectResult(applicants);
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

        [HttpGet("{applicantId}")]
        public ActionResult<Applicant> GetApplicant([FromRoute] int applicantId)
        {
            var applicant = _applicantService.GetApplicant(applicantId);
            if (applicant != null)
            {
                return new OkObjectResult(applicant);
            }

            return new NotFoundObjectResult("Applicant not found");
        }

        [HttpPut("{applicantId}")]
        public ActionResult<Applicant> UpdateApplicant([FromRoute] int applicantId,
            [FromBody] ApplicantDto applicantDto)
        {
            var applicant = _applicantService.UpdateApplicant(applicantId, applicantDto);
            if (applicant != null)
            {
                return new OkObjectResult(applicant);
            }

            return new BadRequestObjectResult("Bad request");
        }

        [HttpDelete("{applicantId}")]
        public ActionResult<Applicant> RemoveApplicant([FromRoute] int applicantId)
        {
            var applicant = _applicantService.RemoveApplicant(applicantId);
            if (applicant != null)
            {
                return new OkObjectResult("applicant deleted");
            }

            return new NotFoundObjectResult("Applicant not found!");
        }

    }
}