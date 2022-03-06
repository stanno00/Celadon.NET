namespace VaccineTask.Models
{
    public class HospitalVaccine
    {
        
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int VaccineId { get; set; }
        public Vaccine Vaccine { get; set; }
        public int NumberOfVaccines { get; set; }
    }
}