﻿namespace DotNetTribes.Models
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public long Started_at { get; set; }
        public long Finished_at { get; set; }
        public int KingdomId { get; set; }


    }
}