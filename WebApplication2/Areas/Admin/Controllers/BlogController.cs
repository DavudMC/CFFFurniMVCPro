using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var blogs = await context.Blogs.Include(x=>x.Employee).ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            context.Blogs.Remove(blog);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await context.Blogs.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var employees = await context.Employees.ToListAsync();
            ViewBag.Employees = employees;

            return View(product);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                var employees = await context.Employees.ToListAsync();
                ViewBag.Employees = employees;
                return View();
            }
            var existingBlog = await context.Blogs.FindAsync(blog.Id);
            if (existingBlog == null)
            {
                return NotFound();
            }
            existingBlog.Title = blog.Title;
            existingBlog.Text = blog.Text;
            existingBlog.UpdatedDate = DateTime.Now;
            existingBlog.PostedDate = blog.PostedDate;
            existingBlog.EmployeeId = blog.EmployeeId;
            existingBlog.ImageName = blog.ImageName;
            context.Blogs.Update(existingBlog);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var blogs = await context.Blogs.ToListAsync();

            var employees = await context.Employees.ToListAsync();

            ViewBag.Employees = employees;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                var employees = await context.Employees.ToListAsync();
                ViewBag.Employees = employees;
                return View();
            }


            blog.CreatedDate = DateTime.Now;
            await context.Blogs.AddAsync(blog);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
