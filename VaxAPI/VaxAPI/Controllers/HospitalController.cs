using Microsoft.AspNetCore.Mvc;
using VaxAPI.DTOs;
using VaxAPI.Models;
using VaxAPI.Services;

namespace VaxAPI.Controllers
{
    [ApiController]
    [Route("hospital")]
    public class HospitalController
    {
        private readonly IHospitalService _hs;
        private readonly IVaccineService _vs;

        public HospitalController(IHospitalService hs, IVaccineService vs)
        {
            _hs = hs;
            _vs = vs;
        }


        [HttpPost("add")]
        public ActionResult AddNewHospital([FromBody] HospitalDTO incoming)
        {
            // If block tries adding the hospital and based on the result (true/false) we send an appropriate response.
            if (!_hs.AddNewHospital(incoming))
            {
                return new BadRequestObjectResult(new ErrorJSON("The hospital could not be created."));
            }

            return new CreatedResult("", new SuccessJSON("Hospital successfully created."));
        }

        [HttpPut("updateName/{hospitalId:long}")]
        public ActionResult UpdateHospitalName([FromBody] HospitalDTO incoming, [FromRoute] long hospitalId)
        {
            if (!_hs.ChangeHospitalName(incoming, hospitalId))
            {
                return new BadRequestObjectResult(new ErrorJSON("The name could not be changed."));
            }

            return new OkObjectResult(new SuccessJSON("Name successfully changed."));
        }

        [HttpPost("orderVaccines")]
        public ActionResult OrderVaccinesForHospital([FromBody] VaccineOrderDTO incoming)
        {
            // the next two lines have in-built checks for exceptions.   
            long? orderId = _vs.AddNewVaccineOrder(incoming);
            VaccineOrder order = _vs.GetOrderById(orderId.Value);

            if (order == null)
            {
                
                return new BadRequestObjectResult(new ErrorJSON("The order could not be created"));
            }

            return _hs.OrderVaccines(order) switch
            {
                1 => new NotFoundObjectResult(new ErrorJSON("Hospital not found")),
                2 => new BadRequestObjectResult(new ErrorJSON("The hospital does not have enough money.")),
                3 => new OkObjectResult(new SuccessJSON("Vaccines successfully ordered.")),
                _ => new BadRequestObjectResult(new ErrorJSON("Unknown error. Please contact the administrator."))
            };

           
        }
        
        [HttpGet("checkVaccines/{hospitalId:long}")]
        public ActionResult CheckVaccinesInHospital([FromRoute] long hospitalId)
        {
            var result = _hs.CheckVaccinesInHospital(hospitalId);
            if (result == null)
            {
                return new BadRequestObjectResult(new ErrorJSON("Hospital not found."));
            }
            
            return new OkObjectResult(new StockDTO
            {
                HospitalName = _hs.GetHospitalName(hospitalId),
                Stock = result
            });
        }
    }
}