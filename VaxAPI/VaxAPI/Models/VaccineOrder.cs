using System.ComponentModel.DataAnnotations.Schema;

namespace VaxAPI.Models
{
    public class VaccineOrder
    {
        public long VaccineOrderId { get; set; }
        
        public Vaccine Vaccine { get; set; }
        public long VaccineId { get; set; }
        public Hospital Hospital { get; set; }
        public long HospitalId { get; set; }
        public int OrderAmount{ get; set; }
        public int TotalPrice => Vaccine.Price * OrderAmount;
    }
}