using Microsoft.EntityFrameworkCore;

namespace VaccineTask
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}