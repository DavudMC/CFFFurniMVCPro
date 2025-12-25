using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
