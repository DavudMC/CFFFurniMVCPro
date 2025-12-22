using Microsoft.AspNetCore.Mvc;
using WebApplication2.Context;

namespace WebApplication2.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var employees = appDbContext.Employees.ToList();

            return View(employees);
        }
    }
}
