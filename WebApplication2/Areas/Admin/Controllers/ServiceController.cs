using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Helper;
using WebApplication2.Models;
using WebApplication2.ViewModels.ServiceViewModel;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController(AppDbContext _context,IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        
        {
            var services = await _context.Services.Select(service => new GetServiceVM() 
            {
                Id = service.Id,
                 Name = service.Name,
                 Description = service.Description,
                 ImageName = service.ImageName,
                 ImagePath = service.ImagePath,

            }).ToListAsync();


            return View(services);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetEmployeesToView();

            return View();
        }

        private async Task GetEmployeesToView()
        {
            var employees = await _context.Employees.ToListAsync();

            ViewBag.Employees = employees;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM vm)
        {
            await GetEmployeesToView();
            if (!ModelState.IsValid)
            {
                
                return View(vm);
            }

            foreach (var empId in vm.EmployeeIds)
            {
                var IsExistEmployeId = await _context.Employees.AnyAsync(x => x.Id == empId);

                if (!IsExistEmployeId)
                {
                    await GetEmployeesToView();
                    ModelState.AddModelError("EmployeeIds", "Bele bir employee movcud deil! ");
                    return View(vm);
                }

            }

            if (vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("Image", "Max size 2mb olmalidir.");
                return View(vm);
            }

            if (!vm.Image.CheckType())
            {
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
            var imagePath = Path.Combine(_environment.WebRootPath, "assets", "images", uniqueImageName);

            using var stream = new FileStream(imagePath, FileMode.Create);
            await vm.Image.CopyToAsync(stream);

            Service service = new()
            {
                
                Name = vm.Name,
                Description = vm.Description,
                ImageName = uniqueImageName,
                EmployeeServices = []
            };

            foreach (var empId in vm.EmployeeIds)
            {
                EmployeeService employeeService = new()
                {
                    EmployeeId = empId,
                    Service = service
                };

                service.EmployeeServices.Add(employeeService);
            }

            service.CreatedDate = DateTime.Now;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var service = await _context.Services.Include(x => x.EmployeeServices)
                                                .FirstOrDefaultAsync(x => x.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            await GetEmployeesToView();

            UpdateServiceVM vm = new UpdateServiceVM
            {
                
                Name = service.Name,
                Description = service.Description,
                ImageName = service.ImageName,
                EmployeeIds = service.EmployeeServices.Select(x => x.EmployeeId).ToList(),
            };


            return View(vm);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateServiceVM vm)
        {
            if (!ModelState.IsValid)
            {
                await GetEmployeesToView();
                return View(vm);
            }

            if (!vm.Image?.CheckType() ?? false)
            {
                await GetEmployeesToView();
                ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
                return View(vm);
            }

            if (vm.Image?.CheckSize(2) ?? false)
            {
                await GetEmployeesToView();
                ModelState.AddModelError("Image", "Max size 2mb olmalidir.");
                return View(vm);
            }

            var existingService = await _context.Services.Include(x => x.EmployeeServices).FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existingService == null)
            {
                await GetEmployeesToView();
                return NotFound();
            }


            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

            if (vm.Image is { })
            {
                string uniqueImageName = await vm.Image.SaveFileAsync(folderPath);

                string oldImagePath = Path.Combine(folderPath, existingService.ImageName);

                ExtensionMethod.DeleteFile(oldImagePath);

                existingService.ImageName = uniqueImageName;

            }

            existingService.Name = vm.Name;
            existingService.Description = vm.Description;
            existingService.UpdatedDate = DateTime.Now;
            existingService.EmployeeServices.Clear();


            if (vm.EmployeeIds is not null)
            {
                foreach (var empId in vm.EmployeeIds)
                {
                    EmployeeService employeeService = new EmployeeService()
                    {
                        EmployeeId = empId,
                        ServiceId = existingService.Id
                    };


                    existingService.EmployeeServices.Add(employeeService);
                }
            }

            _context.Services.Update(existingService);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
