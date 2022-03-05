using Microsoft.AspNetCore.Mvc;
using VaxAPI.DTOs;
using VaxAPI.Models;
using VaxAPI.Services;

namespace VaxAPI.Controllers
{
    [ApiController]
    [Route("vaccine")]
    public class VaccineController
    {
        private readonly IVaccineService _vs;

        public VaccineController(IVaccineService vs)
        {
            _vs = vs;
        }
        
        [HttpPost("add")]
        public ActionResult AddNewVaccine([FromBody] VaccineDTO incoming)
        {
            if(!_vs.CreateNewVaccine(incoming))
            {
                return new BadRequestObjectResult(new ErrorJSON("The vaccine could not be added."));
            }
            return new CreatedResult("", new SuccessJSON("Vaccine successfully added."));
        }
        
        [HttpGet("find/{vaccineId:long}")]
        public ActionResult GetVaccineById([FromRoute] long vaccineId)
        {
            Vaccine vaccine = _vs.GetVaccineById(vaccineId);
            if(vaccine == null)
            {
                return new BadRequestObjectResult(new ErrorJSON("Vaccine not found."));
            }

            return new OkObjectResult(new VaccineDTO {Name = vaccine.Name, Price = vaccine.Price});
        }
        
        [HttpDelete("delete/{vaccineId:long}")]
        public ActionResult DeleteVaccine([FromRoute] long vaccineId)
        {
            if(!_vs.DeleteVaccine(vaccineId))
            {
                return new BadRequestObjectResult(new ErrorJSON("The vaccine could not be deleted."));
            }

            return new OkObjectResult(new SuccessJSON("Vaccine successfully deleted."));
        }
        
        [HttpPut("update/{vaccineId:long}")]
        public ActionResult UpdateVaccineName([FromBody]VaccineDTO incoming, [FromRoute] long vaccineId)
        {
            if(!_vs.UpdateVaccineName(incoming, vaccineId))
            {
                return new BadRequestObjectResult(new ErrorJSON("The name could not be updated"));
            }

            return new OkObjectResult(new SuccessJSON("Name successfully changed."));
        }
        
        
    }
}