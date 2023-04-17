using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
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
            adminViewModel.userList = _cI_PlatformContext.Users.ToList();
            return adminViewModel;
        }

        public User FetchUserDetails(long userId)
        {
            return _cI_PlatformContext.Users.Find(userId);
        }

        public bool AddUpdateUserDetails(AdminViewModel vm)
        {
            if(vm.userDetails.UserId == 0)
            {
                User u = new User();
                u.FirstName = vm.userDetails.FirstName;
                u.LastName = vm.userDetails.LastName;
                u.Email = vm.userDetails.Email;
                u.Password = vm.userDetails.Password;
                u.Avatar = vm.userDetails.Avatar;
                u.EmployeeId = vm.userDetails.EmployeeId;
                u.Department = vm.userDetails.Department;
                u.CityId = vm.userDetails.CityId;
                u.CountryId = vm.userDetails.CountryId;
                u.ProfileText = vm.userDetails.ProfileText;
                u.Status = vm.userDetails.Status;

                _cI_PlatformContext.Users.Add(u);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            else
            {
                User u = _cI_PlatformContext.Users.Find(vm.userDetails.UserId);
                u.FirstName = vm.userDetails.FirstName;
                u.LastName = vm.userDetails.LastName;
                u.Email = vm.userDetails.Email;
                u.Password = vm.userDetails.Password;
                u.Avatar = vm.userDetails.Avatar;
                u.EmployeeId = vm.userDetails.EmployeeId;
                u.Department = vm.userDetails.Department;
                u.CityId = vm.userDetails.CityId;
                u.CountryId = vm.userDetails.CountryId;
                u.ProfileText = vm.userDetails.ProfileText;
                u.Status = vm.userDetails.Status;

                _cI_PlatformContext.Update(u);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
        }
    }
}
