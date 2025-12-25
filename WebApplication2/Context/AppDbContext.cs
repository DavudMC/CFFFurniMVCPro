using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication2.Models;

namespace WebApplication2.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Models.Blog> Blogs { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
