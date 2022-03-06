using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VaccineTask.DTOs;
using VaccineTask.Models;
using VaccineTask.Services;

namespace VaccineTask.Controllers
{
    [ApiController]
    [Route("/applicant/{applicantId}/[controller]")]
    public class ApplicationController
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicantService _applicantService;

        public ApplicationController(IApplicationService applicationService, IApplicantService applicantService)
        {
            _applicationService = applicationService;
            _applicantService = applicantService;
        }

        [HttpGet("{applicationId}")]
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

        [HttpPost]
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

            var application = _applicationService.PostApplication(applicantId, applicationDto);
            if (application == null)
            {
                return new BadRequestObjectResult("Wrong input!");
            }

            return new CreatedResult("", application);

        }
    }
}