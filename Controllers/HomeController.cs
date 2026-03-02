using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementSystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();   // Welcome page only
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}