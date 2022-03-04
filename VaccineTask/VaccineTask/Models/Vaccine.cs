using System.Collections.Generic;

namespace VaccineTask.Models
{
    public class Vaccine
    {
        public int VaccineId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public List<HospitalVaccine> HospitalVaccines { get; set; }
    }
}