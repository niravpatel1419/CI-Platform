using CI_Platform_Web.Data;
using CI_Platform_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CI_PlatformContext _cI_PlatformContext;

        public HomeController(ILogger<HomeController> logger, CI_PlatformContext cI_PlatformContext)
        {
            _logger = logger;
            _cI_PlatformContext = cI_PlatformContext;
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
            User user = new User();
            return View(user);
        }


        [HttpPost]
        public IActionResult registration(User user)
        {
            var compare = _cI_PlatformContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (compare != null)
            {

                ViewBag.RegEmail = "email exist";
            }
            else
            {
                _cI_PlatformContext.Users.Add(user);
                _cI_PlatformContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
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

        public IActionResult storyListingpage()
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