namespace VaxAPI.DTOs
{
    public class VaccineOrderDTO
    {
        public long VaccineId { get; set; }
        public long HospitalId { get; set; }
        public int OrderAmount { get; set; }
    }
}