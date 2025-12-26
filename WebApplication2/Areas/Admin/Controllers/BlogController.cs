using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels.BlogViewModel;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController(AppDbContext context, IWebHostEnvironment enviroment) : Controller
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
        public async Task<IActionResult> Create(CreateBlogVm vm)
        {
            await GetEmployeeWithViewBag();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (vm.Image.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("Image", "Please select image file");
                return View();
            }

            if(vm.Image.Length > 2 *1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image size must be less than 2MB");
                return View();
            }

            string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
            var imagePath = Path.Combine(enviroment.WebRootPath, "assets", "images", uniqueImageName);

            using var stream = new FileStream(imagePath, FileMode.Create);
            await vm.Image.CopyToAsync(stream);


            var blog = new Blog
            {
                Title = vm.Title,
                Text = vm.Text,
                EmployeeId = vm.EmployeeId,
                PostedDate = vm.PostedDate,
                ImageName = uniqueImageName,
                CreatedDate = DateTime.Now,
                ImageUrl = imagePath,
            };

            blog.CreatedDate = DateTime.Now;
            await context.Blogs.AddAsync(blog);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task GetEmployeeWithViewBag()
        {
            var employees = await context.Employees.ToListAsync();
            ViewBag.Employees = employees;
        }
    }
}
