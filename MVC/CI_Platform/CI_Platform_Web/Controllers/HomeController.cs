using CI_Platform_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult forgotPassword()
        {
            return View();
        }

        public IActionResult resetPassword()
        {
            return View();
        }

        public IActionResult registration()
        {
            return View();
        }

        public IActionResult home()
        {
            return View();
        }

        public IActionResult noMissionFound()
        {
            return View();
        }

        public IActionResult volunteeringMission()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}