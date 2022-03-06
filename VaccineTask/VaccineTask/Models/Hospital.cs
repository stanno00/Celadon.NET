using System.Collections.Generic;

namespace VaccineTask.Models
{
    public class Hospital
    {
        public int HospitalId { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }

        public List<VaccineOrder> VaccineOrders { get; set; } = new List<VaccineOrder>();

        public List<HospitalVaccine> HospitalVaccines { get; set; } = new List<HospitalVaccine>();
    }
}