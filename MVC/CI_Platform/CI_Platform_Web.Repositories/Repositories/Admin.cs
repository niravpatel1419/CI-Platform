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
        
        //For Add Or Edit User Details On Admin Page
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

        //For Add or Edit CMS Details on Admin Page
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

        //For Delete the CMS in Admin Page
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




        //For Mission Page

        public List<Mission> FetchMissionDetails()
        {
            return _cI_PlatformContext.Missions.Where(mission => mission.DeletedAt == null).ToList();
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
        
        //For Add or Edit Mission In Admin Page
        public bool AddUpdateMissionDetails(AdminMissionViewModel vm)
        {
            if (vm.mission.MissionId == 0)
            {
                Mission mission = new Mission();
                mission.ThemeId = vm.mission.ThemeId;
                mission.CountryId = vm.mission.CountryId;
                mission.CityId = vm.mission.CityId;
                mission.Title = vm.mission.Title;
                mission.ShortDescription = vm.mission.ShortDescription;
                mission.Description = vm.mission.Description;
                mission.StartDate = vm.mission.StartDate;
                mission.EndDate = vm.mission.EndDate;
                mission.MissionType = vm.mission.MissionType;
                mission.OrganizationName = vm.mission.OrganizationName;
                mission.OrganizationDetail = vm.mission.OrganizationDetail;
                mission.Availability = vm.mission.Availability;
                mission.Seatleft = vm.mission.Seatleft;

                _cI_PlatformContext.Missions.Add(mission);
                _cI_PlatformContext.SaveChanges();
                if (vm.missionSkills.Any())
                {
                    long missionId = _cI_PlatformContext.Missions.OrderByDescending(x => x.MissionId).FirstOrDefault().MissionId;
                    List<MissionSkill> skils= new List<MissionSkill>();
                    foreach(var i in vm.missionSkills)
                    {
                        skils.Add(new MissionSkill
                            {
                            MissionId = missionId,
                            SkillId = i
                        }
                        
                            );
                    }
                    _cI_PlatformContext.MissionSkills.AddRange(skils);
                    _cI_PlatformContext.SaveChanges();
                }
                return true;
            }
            
            else
            {
                Mission mission = _cI_PlatformContext.Missions.Find(vm.mission.MissionId);
                mission.ThemeId = vm.mission.ThemeId;
                mission.CountryId = vm.mission.CountryId;
                mission.CityId = vm.mission.CityId;
                mission.Title = vm.mission.Title;
                mission.ShortDescription = vm.mission.ShortDescription;
                mission.Description = vm.mission.Description;
                mission.StartDate = vm.mission.StartDate;
                mission.EndDate = vm.mission.EndDate;
                mission.MissionType = vm.mission.MissionType;
                mission.OrganizationName = vm.mission.OrganizationName;
                mission.OrganizationDetail = vm.mission.OrganizationDetail;
                mission.Availability = vm.mission.Availability;
                mission.Seatleft = vm.mission.Seatleft;
                mission.UpdatedAt = DateTime.Now;

                _cI_PlatformContext.Missions.Update(mission);
                _cI_PlatformContext.SaveChanges();

                if (vm.missionSkills.Any())
                {
                    long missionId = vm.mission.MissionId;
                    List<MissionSkill> skils = new List<MissionSkill>();
                    foreach (var i in vm.missionSkills)
                    {
                        skils.Add(new MissionSkill
                        {
                            MissionId = missionId,
                            SkillId = i
                        }

                            );
                    }
                    _cI_PlatformContext.MissionSkills.AddRange(skils);
                    _cI_PlatformContext.SaveChanges();
                }
                return true;
            }

        }

        //For Delete the Mission In Admin Page
        public bool DeleteMission(long missionId)
        {
            Mission mission = _cI_PlatformContext.Missions.Find(missionId);
            if (mission != null)
            {
                mission.DeletedAt = DateTime.Now;
                mission.Status = 0;
                _cI_PlatformContext.Missions.Update(mission);
                _cI_PlatformContext.SaveChanges();

                return true;
            }
            return false;
        }



        //For Mission Theme Page On Admin Side
        public List<MissionTheme> FetchMissionThemes()
        {
            return _cI_PlatformContext.MissionThemes.Where(missionthemes => missionthemes.DeletedAt == null).ToList();
        }

        public MissionTheme GetThemeDetail(long themeId)
        {
            return _cI_PlatformContext.MissionThemes.Find(themeId);
        }

        public bool AddEditMissionTheme(AdminMissionThemeViewModel themeViewModel)
        {
            if(themeViewModel.theme.MissionThemeId == 0)
            {
                MissionTheme missionTheme = new MissionTheme();
                missionTheme.Title = themeViewModel.theme.Title;
                missionTheme.Status = themeViewModel.theme.Status;

                _cI_PlatformContext.MissionThemes.Add(missionTheme);
                _cI_PlatformContext.SaveChanges();

                return true;
            }
            else
            {
                MissionTheme missionTheme = _cI_PlatformContext.MissionThemes.Find(themeViewModel.theme.MissionThemeId);
                missionTheme.Title = themeViewModel.theme.Title;
                missionTheme.Status = themeViewModel.theme.Status;
                missionTheme.UpdatedAt = DateTime.Now;

                _cI_PlatformContext.MissionThemes.Update(missionTheme);
                _cI_PlatformContext.SaveChanges();

                return true;
            }
        }

        public bool DeleteTheme(long themeId)
        {
            MissionTheme missionTheme = _cI_PlatformContext.MissionThemes.Find(themeId);
            missionTheme.DeletedAt = DateTime.Now;
            missionTheme.Status = 0;

            _cI_PlatformContext.MissionThemes.Update(missionTheme);
            _cI_PlatformContext.SaveChanges();

            return true;
        }
    }
}