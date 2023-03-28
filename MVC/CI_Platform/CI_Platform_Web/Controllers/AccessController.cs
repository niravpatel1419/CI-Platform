using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CI_Platform_Web.Repositories.Repositories;

namespace CI_Platform_Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly CI_PlatformContext _cI_PlatformContext;
        private readonly ICI_Platform _iCiPlat;

        public AccessController(CI_PlatformContext cI_PlatformContext, ICI_Platform iCiPlat)
        {
            _cI_PlatformContext = cI_PlatformContext;
            _iCiPlat = iCiPlat;
        }

        //For the User Login 

        [AllowAnonymous]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {

                return RedirectToAction("home", "Home");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(User userLogin)
        {
            if (userLogin.Email == null || userLogin.Password == null)
            {
                ViewData["ValidateMessage"] = "Email id or Password Can not be Empty";
                return View();

            }
            else
            {
                User u = new User();
                u = _iCiPlat.Login(userLogin);

                if (u != null)
                {
                    List<Claim> claims = new List<Claim>()
                     {
                        new Claim(ClaimTypes.NameIdentifier, u.Email),
                        new Claim(ClaimTypes.Name, u.FirstName),
                        new Claim(ClaimTypes.Sid, u.UserId.ToString()),
                        new Claim("Other Propert","Exaple role")
                     };


                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                    return RedirectToAction(nameof(HomeController.home), "Home");
                }
            }
            ViewData["ValidateMessage"] = "Email id or Password does not match";
            return View();
        }





        //For the User Registration

        [HttpGet]
        public IActionResult registration()
        {
            return View();
        }


        [HttpPost]
        public IActionResult registration(User r)
        {

            if (_iCiPlat.IsRegister(r))
            {
                return RedirectToAction("Login", "Access");
            }
            else
            {
                TempData["UserExist"] = "Email Address Already Exists.Please use a different email address";
                return View(r);
            }

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
            if (_forogtpass.Email == null)
            {
                return View();
            }

            bool isEmailExist = _iCiPlat.IsEmailExist(_forogtpass.Email);
            if (!isEmailExist)
            {
                ViewData["emailNotFound"] = "EMAIL ID IS INVALID";
                return View();
            }

            // Generate a password reset token for the user
            var token = Guid.NewGuid().ToString();

            // Send an email with the password reset link to the user's email address
            var resetLink = Url.Action("resetPassword", "Access", new { email = _forogtpass.Email, token }, Request.Scheme);

            var fromAddress = new MailAddress("computerengineermeet@gmail.com", "CI_Platform");
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
                Credentials = new NetworkCredential("computerengineermeet@gmail.com", "kknqzbwyupzddahv"),
                EnableSsl = true
            };
            smtpClient.Send(message);

            _iCiPlat.ForgetPassword(_forogtpass.Email, token);

            return RedirectToAction("ForgotPasswordConfirmation", "Access");

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

            var user = _cI_PlatformContext.Users.FirstOrDefault(m => m.Email == model.Email);
            if (user == null)
            {
                ViewData["passwordResetFail"] = "Token or Email is Invalid";
                return View();

            }

            user.Password = model.Password;
            _cI_PlatformContext.SaveChanges();
            ViewData["AlertMessage"] = "Your Password Changed Successfully...! Login with the new password now.";
            return RedirectToAction("Login", "Access");

        }


        //For Logout

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
