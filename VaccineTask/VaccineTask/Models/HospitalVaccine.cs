using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineTask.Models
{
    public class HospitalVaccine
    {
        
        public int HospitalId { get; set; }
        
        public string HospitalName { get; set; }
        
        public int VaccineId { get; set; }
        
        public string VaccineName { get; set; }
        
        public int NumberOfVaccines { get; set; }
    }
}