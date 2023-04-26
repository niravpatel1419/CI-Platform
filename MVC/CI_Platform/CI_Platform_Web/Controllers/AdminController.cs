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

        public IActionResult User()
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
        
        public IActionResult AddUpdateUserDetails(AdminViewModel vm)
        {
            bool b = _iAdmin.AddUpdateUserDetails(vm);
            return RedirectToAction("user", "admin");
        }


        //For CMS Page

        public IActionResult CmsPage()
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
        public IActionResult Mission()
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

            if (true)
            {
                bool missionDetails = _iAdmin.AddUpdateMissionDetails(vm);
            }
            

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

        public IActionResult MissionTheme()
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
            if(vm.theme.Title == null)
            {
                TempData["error"] = "Test error";
            }

            ViewBag.SelectedTab = "mtheme";
            bool theme = _iAdmin.AddEditMissionTheme(vm);
            return RedirectToAction("missionTheme", "Admin");
        }

        public bool DeleteMissionTheme(long missionThemeId)
        {
            return _iAdmin.DeleteTheme(missionThemeId);
        }




        //For Mission Skill Page

        public IActionResult MissionSkills()
        {
            ViewData["admin-Title"] = "Mission Skills";
            ViewBag.SelectedTab = "mskill";
            AdminMissionSkillsViewModel skill = new AdminMissionSkillsViewModel();
            skill.skills = _iAdmin.FetchSkillDetails();
            return View(skill);
        }

        public Skill GetSkillDetail(long missionSkillId)
        {
            Skill skill = _iAdmin.GetSkillDetail(missionSkillId);
            return skill;
        }

        public IActionResult AddEditSkill(AdminMissionSkillsViewModel skillVm)
        {
            ViewBag.SelectedTab = "mskill";
            bool skill = _iAdmin.AddEditSkill(skillVm);
            return RedirectToAction("missionSkills","Admin");
        }

        public bool DeleteMissionSkill(long missionSkillId)
        {
            return _iAdmin.DeleteSkill(missionSkillId);
        }



        //For Mission Application Page
        public IActionResult MissionApplication()
        {
            ViewData["admin-Title"] = "Mission Application";
            ViewBag.SelectedTab = "mapplication";
            AdminMissionApplicationViewModel adminMissionApplication = new AdminMissionApplicationViewModel();
            adminMissionApplication.missionApplications = _iAdmin.FetchMissionApplication();
            adminMissionApplication.users = _iAdmin.FetchUser();
            adminMissionApplication.missions = _iAdmin.FetchMissionDetails();
            return View(adminMissionApplication);
        }

        public IActionResult ApproveRejectMissionApplication(int status,long missionApplicationId)
        {
            bool result = _iAdmin.ApproveRejectMissionApplication(status,missionApplicationId);
            return RedirectToAction("MissionApplication","Admin");
        }




        //For Sotry Page

        public IActionResult Story()
        {
            ViewData["admin-Title"] = "Story";
            ViewBag.SelectedTab = "story";
            List<AdminStoryViewModel> vm = _iAdmin.FetchStoryList();
            return View(vm);
        }

        public IActionResult GetStoryDetails(long storyId)
        {

            AdminStoryViewModel v = _iAdmin.GetStoryDetails(storyId);
            var data = new
            {
                story = v.storyDetails, 
                media = v.storyimages
            };
            return Json(data);

        }

        public bool ApproveRejectStory(int status, long storyId)
        {
            return _iAdmin.StoryStatus(status, storyId);
        }

        public bool DeleteStory(long storyId)
        {
            return _iAdmin.DeleteStory(storyId);
        }






        public IActionResult BannerManagement()
        {
            ViewData["admin-Title"] = "User";
            ViewBag.SelectedTab = "banner";
            return View();
        }
    }
}
