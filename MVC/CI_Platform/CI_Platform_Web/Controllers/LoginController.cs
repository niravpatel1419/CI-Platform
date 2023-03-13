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
                    return RedirectToAction(nameof(HomeController.home), "Home");
                }
                else
                {
                    return RedirectToAction(nameof(LoginController.Login), "Home");
                }
            }
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

}








/*[HttpPost]

public IActionResult Login(Login model)
{
    if (ModelState.IsValid)
    {

        var user = _cI_PlatformContext.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

        if (user != null)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) },
                CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.LastName));
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetString("EmailId", user.Email);

            return RedirectToAction(nameof(HomeController.home), "Home");

        }

        else
        {
            TempData["Message"] = "Email or Password is incorrect";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
    return View();
}*/