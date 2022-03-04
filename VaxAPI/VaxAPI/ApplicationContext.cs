using Microsoft.EntityFrameworkCore;
using VaxAPI.Models;

namespace VaxAPI
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<VaccineOrder> VaccineOrders { get; set; }


        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HospitalStock>()
                .HasKey(k => new {k.HospitalId, k.VaccineId});
            
        }
    }
}