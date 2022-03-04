﻿namespace VaccineTask.Models
{
    public class VaccineOrder
    {
        public int VaccineOrderId { get; set; }
        public string VaccineName { get; set; }
        public string HospitalName { get; set; }
        public int NumberOfVaccinesBeingOrdered { get; set; }
        public int TotalPriceOfVaccines { get; set; }

        public int VaccineId { get; set; }
        public int HospitalId { get; set; }
    }
}