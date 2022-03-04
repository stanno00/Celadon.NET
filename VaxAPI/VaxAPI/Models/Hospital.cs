using System.Collections;
using System.Collections.Generic;

namespace VaxAPI.Models
{
    public class Hospital
    {
        public long HospitalId { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }
        public ICollection<HospitalStock> HospitalStock { get; set; }
        
        
    }
}