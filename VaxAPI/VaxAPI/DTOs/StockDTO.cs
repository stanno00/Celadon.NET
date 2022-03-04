using System.Collections.Generic;

namespace VaxAPI.DTOs
{
    public class StockDTO
    {
        public string HospitalName { get; set; }
        public Dictionary<string, int> Stock { get; set; }
    }
}