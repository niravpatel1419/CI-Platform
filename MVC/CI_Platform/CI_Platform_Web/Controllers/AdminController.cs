using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using CI_Platform_Web.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly CI_PlatformContext _cI_PlatformContext;
        private readonly IAdmin _iAdmin;

        public AdminController(CI_PlatformContext cI_PlatformContext, IAdmin iAdmin)
        {
            _cI_PlatformContext = cI_PlatformContext;
            _iAdmin = iAdmin;
        }

        public IActionResult user()
        {
            var userDetails = _iAdmin.GetUserDetails();
            return View(userDetails);
        }

        public User GetUserDetail(long userID)
        {
            User u = _iAdmin.FetchUserDetails(userID);
            return u;
        }
        
        //For ADD or UPDATE the user details

        public IActionResult addUpdateUserDetails(AdminViewModel vm)
        {
            bool b = _iAdmin.AddUpdateUserDetails(vm);
            return RedirectToAction("user", "admin");
        }

        public IActionResult cmsPage()
        {
            return View();
        }

        public IActionResult mission()
        {
            return View();
        }

        public IActionResult missionTheme()
        {
            return View();
        }

        public IActionResult missionSkills()
        {
            return View();
        }

        public IActionResult missionApplication()
        {
            return View();
        }

        public IActionResult story()
        {
            return View();
        }

        public IActionResult bannerManagement()
        {
            return View();
        }
    }
}
