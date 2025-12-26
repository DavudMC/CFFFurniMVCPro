using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;

namespace WebApplication2.Controllers
{
    public class BlogController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var blogs = await context.Blogs.Include(b=>b.Employee).ToListAsync();
            return View(blogs);
        }
    }
}


