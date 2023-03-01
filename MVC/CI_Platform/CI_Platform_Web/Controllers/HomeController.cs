using CI_Platform_Web.Data;
using CI_Platform_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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


        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {

                var user = await _cI_PlatformContext.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefaultAsync();

                if (user != null)
                {

                    return RedirectToAction(nameof(HomeController.home), "Home");

                }
                else
                {
                    TempData["Message"] = "Email or Password is incorrect";
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
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
                
                ViewBag.RegEmail = "Email Address Already Exists.Please use a different email address";

            }
            else
            {
                _cI_PlatformContext.Users.Add(user);
                _cI_PlatformContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
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