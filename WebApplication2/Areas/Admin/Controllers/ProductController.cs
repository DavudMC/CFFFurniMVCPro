using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;
using WebApplication2.ViewModels.ProductViewModel;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context, IWebHostEnvironment environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            return View(products);
        }

        public IActionResult Toggle(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            product.IsDeleted = !product.IsDeleted;
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.ImageName = product.ImageName;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.UpdatedDate = DateTime.UtcNow;
            _context.Products.Update(existingProduct);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVm vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            if (vm.Image.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Max size 2mb olmalidir.");
                return View(vm);
            }

            string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
            string imagePath = Path.Combine(environment.WebRootPath, "assets", "images", uniqueImageName);

            using FileStream mainStream = new FileStream(imagePath, FileMode.Create);

            await vm.Image.CopyToAsync(mainStream);



            Product product = new Product
            {
                Name = vm.Name,
                Price = vm.Price,
                ImageName = uniqueImageName,
                ImageUrl = "/images/" + vm.Image.FileName,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
