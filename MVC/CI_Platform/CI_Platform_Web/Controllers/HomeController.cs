using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
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


        //For the User Registration

        [HttpGet]
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


        //For the User Login 


        [HttpPost]

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
        }


        //For Logout

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookie in storedCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            //HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        //For the Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }



        //For the ForgotPassword

        [HttpGet]
        public IActionResult forgotPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult forgotPassword(ForgotPassword _forogtpass)
        {

            var status = _cI_PlatformContext.Users.FirstOrDefault(m => m.Email == _forogtpass.Email);
            if (status == null)
            {

                TempData["Error"] = "Email id is invalid or You are not registerd";
                return View();

            }

            var token = Guid.NewGuid().ToString();

            var passwordReset = new CI_Platform_Web.Entities.Models.PasswordReset
            {
                Email = _forogtpass.Email,
                Token = token,
            };

            _cI_PlatformContext.Add(passwordReset);
            _cI_PlatformContext.SaveChanges();

            var resetLink = Url.Action("resetPassword", "Home", new { email = _forogtpass.Email, token }, Request.Scheme);

            var fromAddress = new MailAddress("testbhai393@gmail.com", "CI_Platform");
            var toAddress = new MailAddress(_forogtpass.Email);
            var subject = "Password reset request";
            var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("testbhai393@gmail.com", "fdqbpbjcivvfbayr"),
                EnableSsl = true
            };
            smtpClient.Send(message);

            return RedirectToAction("ForgotPasswordConfirmation", "Home");

        }


        //For ForgotPasswordConfirmation

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        // For ResetPassword

        [HttpGet]
        public IActionResult resetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password Reset Token");
            }
            return View();
        }

        [HttpPost]
        public IActionResult resetPassword(ResetPasswordView model)

        {
            if (ModelState.IsValid)
            {
                var user = _cI_PlatformContext.Users.FirstOrDefault(m => m.Email == model.Email);
                if (user == null)
                {
                    return RedirectToAction("forgotPassword", "Home");

                }

                user.Password = model.Password;
                _cI_PlatformContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("forgotPassword", "Home");
        }


        //For LandingPage

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





        /*        public IActionResult LandingPage()
                {
                    string UserID = HttpContext.Session.GetString("UserID");
                    if (UserID == null)
                    {
                        return RedirectToAction("Login", "Auth");
                    }
                    else
                    {
                        var model = _landingPageRepo.createLandingPageModel(long.Parse(UserID));
                        return View(model);
                    }
                }*/
    }
}






