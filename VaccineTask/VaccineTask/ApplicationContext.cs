using Microsoft.EntityFrameworkCore;
using VaccineTask.Models;

namespace VaccineTask
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<VaccineOrder> VaccineOrders { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HospitalVaccine>()
                .HasKey(hv => new {hv.HospitalId, hv.VaccineId});
        }
    }
}