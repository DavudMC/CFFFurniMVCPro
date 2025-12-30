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

        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }

    }
}
