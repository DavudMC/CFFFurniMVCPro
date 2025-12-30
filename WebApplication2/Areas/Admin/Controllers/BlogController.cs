using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Helper;
using WebApplication2.Models;
using WebApplication2.ViewModels.BlogViewModel;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController(AppDbContext _context, IWebHostEnvironment _enviroment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(x => x.Employee).ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Blogs.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.ToListAsync();
            ViewBag.Employees = employees;

            UpdateBlogVm vm = new UpdateBlogVm
            {
                Id = product.Id,
                Title = product.Title,
                Text = product.Text,
                PostedDate = product.PostedDate,
                EmployeeId = product.EmployeeId,
                ImageName = product.ImageName,
                TagIds = product.BlogTags.Select(x=>x.TagId).ToList()
            };


            return View(vm);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogVm vm)
        {
            if (!ModelState.IsValid)
            {
                var employees = await _context.Employees.ToListAsync();
                ViewBag.Employees = employees;
                return View();
            }

            if (!vm.Image?.CheckType() ?? false)
            {
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.Image?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("Image", "Max size 2mb olmalidir.");
                return View(vm);
            }

            var existingBlog = await _context.Blogs.Include(x=>x.BlogTags).FirstOrDefaultAsync(x=>x.Id == vm.Id);
            if (existingBlog == null)
            {
                return BadRequest();
            }
            foreach(var tagId in vm.TagIds)
            {
                var isexisttagId = await _context.Tags.AnyAsync(x=>x.Id == tagId);
                if(!isexisttagId)
                {
                    await GetEmployeeWithViewBag();
                    ModelState.AddModelError("TagIds", "Bele bir tag movcud deyil.");
                    return View(vm);
                }
            }

            string folderPath = Path.Combine(_enviroment.WebRootPath, "assets", "images");

            if (vm.Image is { })
            {
                string uniqueIMageName = await vm.Image.SaveFileAsync(folderPath);

                string oldImagePath = Path.Combine(folderPath, existingBlog.ImageName);

                ExtensionMethod.DeleteFile(oldImagePath);

                existingBlog.ImageName = uniqueIMageName;

            }


            existingBlog.Title = vm.Title;
            existingBlog.Text = vm.Text;
            existingBlog.UpdatedDate = DateTime.Now;
            existingBlog.PostedDate = vm.PostedDate;
            existingBlog.EmployeeId = vm.EmployeeId;
            _context.Blogs.Update(existingBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var blogs = await _context.Blogs.ToListAsync();

            await GetEmployeeWithViewBag();

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


            foreach(var tagId in vm.TagIds)
            {
                var IsExistTagId = await _context.Tags.AnyAsync(x => x.Id == tagId);

                if (!IsExistTagId)
                {
                    await GetEmployeeWithViewBag();
                    ModelState.AddModelError("TagIds", "Bele bir tag movcud deil! ");
                    return View();
                }

            }


            if (!vm.Image.CheckType())
            {
                ModelState.AddModelError("Image", "Please select image file");
                return View();
            }

            if (vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("Image", "Image size must be less than 2MB");
                return View();
            }



            string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
            var imagePath = Path.Combine(_enviroment.WebRootPath, "assets", "images", uniqueImageName);

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
                BlogTags = []
            };


            foreach (var tagId in vm.TagIds)
            {
                BlogTag blogTag = new()
                {
                    TagId = tagId,
                    Blog = blog
                };

                blog.BlogTags.Add(blogTag);

            }

            blog.CreatedDate = DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task GetEmployeeWithViewBag()
        {
            var employees = await _context.Employees.ToListAsync();

            var tags = await _context.Tags.ToListAsync();

            ViewBag.Employees = employees;

            ViewBag.Tags = tags;
        }
    }
}
