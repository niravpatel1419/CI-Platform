using CI_Platform_Web.Entities.Data;
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

        public IActionResult admin()
        {
            var userDetails = _iAdmin.GetUserDetails();
            return View(userDetails);
        }
    }
}
