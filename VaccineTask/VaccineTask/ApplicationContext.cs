using Microsoft.EntityFrameworkCore;
using VaccineTask.Models;

namespace VaccineTask
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HospitalVaccine>().HasKey(hv => new {hv.HospitalId, hv.VaccineId});
        }
    }
}