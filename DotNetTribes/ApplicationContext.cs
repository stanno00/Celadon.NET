using DotNetTribes.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetTribes
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<User> Users { get; set; }
        
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}