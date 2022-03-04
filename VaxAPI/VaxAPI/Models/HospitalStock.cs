namespace VaxAPI.Models
{
    public class HospitalStock
    {
        public long HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public long VaccineId { get; set; }
        public Vaccine Vaccine { get; set; }
        public int StockAmount{ get; set; }

        public HospitalStock(long hospitalId, long vaccineId)
        {
            HospitalId = hospitalId;
            VaccineId = vaccineId;
            StockAmount = 0;
        }
    }
}