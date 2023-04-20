using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using CI_Platform_Web.Repositories.Interface;
using CI_Platform_Web.Repositories.Repositories;
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

        //For User Page

        public IActionResult user()
        {
            ViewBag.SelectedTab = "user";
            var userDetails = _iAdmin.GetUserDetails();
            return View(userDetails);
        }

        public User GetUserDetail(long userID)
        {
            User u = _iAdmin.FetchUserDetails(userID);
            return u;
        }
        
        public IActionResult addUpdateUserDetails(AdminViewModel vm)
        {
            bool b = _iAdmin.AddUpdateUserDetails(vm);
            return RedirectToAction("user", "admin");
        }


        //For CMS Page

        public IActionResult cmsPage()
        {
            ViewData["admin-Title"] = "CMS Page";
            ViewBag.SelectedTab = "cms";
            CMSViewModel vm = new CMSViewModel();
            vm.cms = _iAdmin.FetchCMSPages();
            return View(vm);
        }

        public bool DeleteCMSPage(long cmsPageId)
        {
            return _iAdmin.DeleteCMS(cmsPageId);
        }

        public IActionResult AddEditCms(long cmsPageId)
        {
            ViewBag.SelectedTab = "cms";
            if (cmsPageId == 0)
            {
                ViewBag.cmsID = 0;
                return View();
            }
            else
            {
                CMSViewModel v = new CMSViewModel();
                v.cmsDetails = _iAdmin.GetCmsPageDetails(cmsPageId);
                ViewBag.cmsID = cmsPageId;
                return View(v);
            }

        }

        [HttpPost]
        public IActionResult AddEditCms(CMSViewModel vm)
        {
            string d = Request.Form["text_editor"].ToString();
            vm.cmsDetails.Description = d;
            ViewBag.SelectedTab = "cms";
            bool b = _iAdmin.AddUpdateCMSDetails(vm);
            return RedirectToAction("cmsPage","Admin");
        }


        //For Mission Page
        public IActionResult mission()
        {
            ViewData["admin-Title"] = "Mission";
            ViewBag.SelectedTab = "mission";
            AdminMissionViewModel missionDetail = new AdminMissionViewModel();
            missionDetail.Missions = _iAdmin.FetchMissionDetails();
            return View(missionDetail);
        }

        public IActionResult AddEditMission(long missionId)
        {
            ViewBag.SelectedTab = "mission";
            if(missionId == 0)
            {
                AdminMissionViewModel v = new AdminMissionViewModel();
                v.countries = _iAdmin.GetCountries();
                v.missionTheme = _iAdmin.GetMissionThemes();
                v.Skill = _iAdmin.GetSkills();
                ViewBag.missionId = 0;
                return View(v);
            }
            else
            {
                AdminMissionViewModel v = new AdminMissionViewModel();
                v.mission = _iAdmin.MissionDetails(missionId);
                v.countries = _iAdmin.GetCountries();
                v.missionTheme = _iAdmin.GetMissionThemes();
                v.Skill = _iAdmin.GetSkills();
                ViewBag.missionId = missionId;
                return View(v);
            }
            
        }

        [HttpPost]
        public IActionResult AddEditMission(AdminMissionViewModel vm)
        {
            ViewBag.SelectedTab = "mission";
            string missionDescription = Request.Form["text_editor"].ToString();
            string organizationDetail = Request.Form["text_editor1"].ToString();
            vm.mission.Description = missionDescription;
            vm.mission.OrganizationDetail = organizationDetail;
            bool missionDetails = _iAdmin.AddUpdateMissionDetails(vm);

            return RedirectToAction("mission","Admin");
        }

        public JsonResult City(int id)
        {
            var city = _cI_PlatformContext.Cities.Where(s => s.CountryId == id).ToList();
            return new JsonResult(city);
        }

        public bool DeleteMission(long missionId)
        {
            return _iAdmin.DeleteMission(missionId);
        }


        //For Mission Theme Page

        public IActionResult missionTheme()
        {
            ViewData["admin-Title"] = "Mission Themes";
            ViewBag.SelectedTab = "mtheme";
            AdminMissionThemeViewModel vm = new AdminMissionThemeViewModel();
            vm.missionThemes = _iAdmin.FetchMissionThemes();
            return View(vm);
        }

        public MissionTheme GetThemeDetail(long missionThemeId)
        {
            MissionTheme theme = _iAdmin.GetThemeDetail(missionThemeId);
            return theme;
        }

        public IActionResult AddEditMissionTheme(AdminMissionThemeViewModel vm)
        {
            ViewBag.SelectedTab = "mtheme";
            bool theme = _iAdmin.AddEditMissionTheme(vm);
            return RedirectToAction("missionTheme", "Admin");
        }

        public bool DeleteMissionTheme(long missionThemeId)
        {
            return _iAdmin.DeleteTheme(missionThemeId);
        }





        public IActionResult missionSkills()
        {
            ViewData["admin-Title"] = "User";
            ViewBag.SelectedTab = "missionSkill";
            return View();
        }

        public IActionResult missionApplication()
        {
            ViewData["admin-Title"] = "User";
            ViewBag.SelectedTab = "missionApplication";
            return View();
        }

        public IActionResult story()
        {
            ViewData["admin-Title"] = "User";
            ViewBag.SelectedTab = "story";
            return View();
        }

        public IActionResult bannerManagement()
        {
            ViewData["admin-Title"] = "User";
            ViewBag.SelectedTab = "banner";
            return View();
        }
    }
}
