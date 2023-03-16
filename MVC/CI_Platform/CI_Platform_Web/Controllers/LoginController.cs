using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CI_Platform_Web.Controllers
{
    public class LoginController : Controller
    {

        private readonly CI_PlatformContext _cI_PlatformContext;

        public LoginController(CI_PlatformContext cI_PlatformContext)
        {
            _cI_PlatformContext = cI_PlatformContext;
        }


        //For the User Login 

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _cI_PlatformContext.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefaultAsync();
                var username = model.Email.Split("@")[0];
                if (user != null)
                {
                    HttpContext.Session.SetString("userID", username);
                    HttpContext.Session.SetInt32("Id", (int)user.UserId);
                    return RedirectToAction(nameof(HomeController.home), "Home");
                }
                else
                {
                    return RedirectToAction(nameof(LoginController.Login), "Home");
                }
            }
            return View();
        }

        //For Logout

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

}
