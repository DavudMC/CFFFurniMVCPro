using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var customers = await context.Customers.ToListAsync();

            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            customer.CreatedDate = DateTime.UtcNow;
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var customer = await context.Customers.FindAsync(id);

            if(customer == null)
            {
                return NotFound();
            }

            return View(customer);

        }

        [HttpPost]
        public async Task<IActionResult> Update(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var existingCustomer =  await context.Customers.FindAsync(customer.Id);

            if(existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Username = customer.Username;
            existingCustomer.Email = customer.Email;
            existingCustomer.Password = customer.Password;

            context.Customers.Update(existingCustomer);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
