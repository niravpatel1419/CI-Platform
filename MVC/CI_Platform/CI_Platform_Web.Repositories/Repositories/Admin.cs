using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using CI_Platform_Web.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
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

        //For User Page

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


        //For the CMS Page

        public List<CmsPage> FetchCMSPages()
        {
            return _cI_PlatformContext.CmsPages.Where(cmspage => cmspage.DeletedAt == null).ToList();
        }

        public CmsPage GetCmsPageDetails(long cmsPageId)
        {
            return _cI_PlatformContext.CmsPages.Find(cmsPageId);
        }

        public bool DeleteCMS(long cmsPageId)
        {
            CmsPage page = _cI_PlatformContext.CmsPages.Find(cmsPageId);
            if (page != null)
            {
                page.DeletedAt = DateTime.Now;
                page.Status = 0;
                _cI_PlatformContext.CmsPages.Update(page);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AddUpdateCMSDetails(CMSViewModel vm)
        {
            if (vm.cmsDetails.CmsPageId == 0)
            {
                CmsPage cms = new CmsPage();
                cms.Title = vm.cmsDetails.Title;
                cms.Description = vm.cmsDetails.Description;
                cms.Slug = vm.cmsDetails.Slug;
                cms.Status = vm.cmsDetails.Status;

                _cI_PlatformContext.CmsPages.Add(cms);
                _cI_PlatformContext.SaveChanges();

                return true;
            }
            else
            {
                CmsPage cms = _cI_PlatformContext.CmsPages.Find(vm.cmsDetails.CmsPageId);
                cms.Title = vm.cmsDetails.Title;
                cms.Description = vm.cmsDetails.Description;
                cms.Slug = vm.cmsDetails.Slug;
                cms.Status = vm.cmsDetails.Status;
                cms.UpdatedAt = DateTime.Now;

                _cI_PlatformContext.CmsPages.Update(cms);
                _cI_PlatformContext.SaveChanges();

                return true;

            }
            
        }


        //For Mission Page
        public AdminMissionViewModel GetMissionDetails()
        {
            AdminMissionViewModel missionDetails = new AdminMissionViewModel();
            missionDetails.Missions = _cI_PlatformContext.Missions.ToList();
            missionDetails.countries = _cI_PlatformContext.Countries.ToList();
            return missionDetails;
        }

        public Mission MissionDetails(long missionId)
        {
            return _cI_PlatformContext.Missions.Find(missionId);
        }

        public List<Country> GetCountries()
        {
            return _cI_PlatformContext.Countries.ToList();
        }

        public List<MissionTheme> GetMissionThemes()
        {
            return _cI_PlatformContext.MissionThemes.ToList();
        }

        public List<Skill> GetSkills()
        {
            return _cI_PlatformContext.Skills.ToList();
        }
    }
}
