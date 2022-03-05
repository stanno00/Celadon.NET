using Microsoft.AspNetCore.Mvc;
using VaxAPI.DTOs;
using VaxAPI.Models;
using VaxAPI.Services;

namespace VaxAPI.Controllers
{
    [ApiController]
    [Route("applicant")]
    public class ApplicantController
    {
        private readonly IApplicantService _aps;

        public ApplicantController(IApplicantService aps)
        {
            _aps = aps;
        }

        [HttpPost("add")]
        public ActionResult AddNewApplicant([FromBody] ApplicantDTO incoming)
        {
            if(!_aps.AddNewApplicant(incoming))
            {
                return new BadRequestObjectResult(new ErrorJSON("The applicant could not be created."));
            }

            return new CreatedResult("", new SuccessJSON("Applicant successfully saved."));
        }
        
    }
}