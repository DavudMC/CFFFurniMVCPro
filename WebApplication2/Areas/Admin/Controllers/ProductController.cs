using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Helper;
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

            UpdateProductVm vm = new UpdateProductVm
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImagePath = product.ImageName
            };


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVm vm)
        {
            if (!ModelState.IsValid)
            {
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


            var existingProduct = await _context.Products.FindAsync(vm.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = vm.Name;
            existingProduct.Price = vm.Price;
            existingProduct.UpdatedDate = DateTime.UtcNow;

            string folderPath = Path.Combine(environment.WebRootPath, "assets", "images");

            if (vm.Image is { })
            {
                string uniqueImageName = await vm.Image.SaveFileAsync(folderPath);

                string existingFilePath = Path.Combine(folderPath, existingProduct.ImageName);

                ExtensionMethod.DeleteFile(existingFilePath);

                existingProduct.ImageName = uniqueImageName;
            }


            _context.Products.Update(existingProduct);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> Create()
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


            if (!vm.Image.CheckType())
            {
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.Image.CheckSize(2))
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
