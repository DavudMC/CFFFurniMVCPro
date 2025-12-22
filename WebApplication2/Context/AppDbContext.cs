using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApplication2.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }

    }
}
