using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.ViewModel;
using CI_Platform_Web.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Repositories.Repositories
{
    public class Admin : IAdmin
    {
        private readonly CI_PlatformContext _cI_PlatformContext;

        public Admin(CI_PlatformContext cI_PlatformContext)
        {
            _cI_PlatformContext = cI_PlatformContext;
        }

        public AdminViewModel GetUserDetails()
        {
            AdminViewModel adminViewModel = new AdminViewModel();
            adminViewModel.userdetails = _cI_PlatformContext.Users.ToList();
            return adminViewModel;
        }
    }
}
