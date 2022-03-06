﻿using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{applicantId}/application/{applicationId}")]
        public ActionResult<Application> GetApplication([FromRoute] int applicantId, [FromRoute] int applicationId)
        {
            var applicant = _applicantService.GetApplicant(applicantId);

            if (applicant == null)
            {
                return new NotFoundObjectResult("Applicant not found!");
            }
            
            var application = _applicantService.GetApplicant(applicantId).Applications
                .FirstOrDefault(a => a.ApplicationId == applicationId);

            if (application == null)
            {
                return new NotFoundObjectResult("Application not found!");
            }

            return new OkObjectResult(application);
        }

        [HttpPost("{applicantId}/application")]
        public ActionResult<Application> PostApplication([FromBody] ApplicationDto applicationDto, [FromRoute] int applicantId)
        {
            var applicant = _applicantService.GetApplicant(applicantId);
            if (applicant == null)
            {
                return new NotFoundObjectResult("Applicant not found!");
            }
            
            var numberOfApplications = applicant.Applications.Count;
            if (numberOfApplications == 3)
            {
                return new BadRequestObjectResult("Applicant has already 3 applications");
            }

            var application = _applicantService.PostApplication(applicantId, applicationDto);
            if (application == null)
            {
                return new BadRequestObjectResult("Wrong input!");
            }

            return new CreatedResult("", application);

        }

    }
}