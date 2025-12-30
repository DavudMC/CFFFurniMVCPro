using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Context;
using WebApplication2.Helper;
using WebApplication2.Models;
using WebApplication2.ViewModels.EmployeeViewModel;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController(AppDbContext _context,IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Select(employee => new GetEmployeeVM() 
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                Description = employee.Description,
                ImageName = employee.ImageName,
                ImageUrl = employee.ImageUrl
            }).ToListAsync();

            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SendServicesToView();
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Create(CreateEmployeeVm vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        await SendServicesToView();
        //        return View();
        //    }
        //    foreach(var serviceId in vm.ServiceIds)
        //    {
        //        var isexistserviceId = await _context.Services.AnyAsync(s => s.Id == serviceId);
        //        if(!isexistserviceId)
        //        {
        //            await SendServicesToView();
        //            ModelState.AddModelError("ServiceIds", "Bele bir service movcud deyil!");
        //            return View(vm);
        //        }
        //    }
        //    if(!vm.Image.CheckType())
        //    {
        //        ModelState.AddModelError("Image", "Sadece sekil formatinda data daxil edile biler!");
        //        return View(vm);
        //    }
        //    if(!vm.Image.CheckSize(2))
        //    {
        //        ModelState.AddModelError("Image", "2 mb-dan artiq data daxil edile bilmez!");
        //        return View(vm);
        //    }
        //    string folderpath = Path.Combine(_environment.WebRootPath, "assets", "images");
        //    string newimagePath = await vm.Image.SaveFileAsync(folderpath);
        //    Employee employee = new()
        //    {
        //        FirstName = vm.FirstName,
        //        LastName = vm.LastName,
        //        Position = vm.Position,
        //        Description = vm.Description,
        //        ImageName = newimagePath,
        //        ImageUrl = newimagePath,
        //        EmployeeServices = []
        //    };


        //}
        public async Task SendServicesToView()
        {
            var services = await _context.Services.ToListAsync();
            ViewBag.Services = services;
        }

    }
}
