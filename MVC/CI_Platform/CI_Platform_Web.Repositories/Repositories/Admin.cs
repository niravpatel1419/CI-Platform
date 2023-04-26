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
            adminViewModel.userList = _cI_PlatformContext.Users.Where(user => user.DeletedAt == null).ToList();
            adminViewModel.countryList = _cI_PlatformContext.Countries.ToList();
            return adminViewModel;
        }

        public User FetchUserDetails(long userId)
        {
            return _cI_PlatformContext.Users.Find(userId);
        }
        
        //For Add Or Edit User Details On Admin Page
        public bool AddUpdateUserDetails(AdminViewModel vm)
        {
            if(vm.UserId == 0)
            {
                User u = new User();
                u.FirstName = vm.FirstName;
                u.LastName = vm.LastName;
                u.Email = vm.Email;
                u.Password = vm.Password;
                /*u.Avatar = vm.userDetails.Avatar;*/
                u.EmployeeId = vm.EmployeeId;
                u.Department = vm.Department;
                u.CityId = vm.CityId;
                u.CountryId = vm.CountryId;
                u.ProfileText = vm.ProfileText;
                u.Status = vm.Status;

                _cI_PlatformContext.Users.Add(u);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            else
            {
                User u = _cI_PlatformContext.Users.Find(vm.UserId);
                u.FirstName = vm.FirstName;
                u.LastName = vm.LastName;
                u.Email = vm.Email;
                u.Password = vm.Password;
               /* u.Avatar = vm.userDetails.Avatar;*/
                u.EmployeeId = vm.EmployeeId;
                u.Department = vm.Department;
                u.CityId = vm.CityId;
                u.CountryId = vm.CountryId;
                u.ProfileText = vm.ProfileText;
                u.Status = vm.Status;

                _cI_PlatformContext.Update(u);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
        }

        //For Delete The User
        public bool DeleteUser(long userId)
        {
            var time = DateTime.Now;
            User user = _cI_PlatformContext.Users.Find(userId);

            List<Comment> comments = _cI_PlatformContext.Comments.Where(comment => comment.UserId == userId).ToList();
            comments.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(comments);

            List<FavouriteMission> favMission = _cI_PlatformContext.FavouriteMissions.Where(fav => fav.UserId == userId).ToList();
            favMission.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(favMission);

            List<MissionApplication> missionapp = _cI_PlatformContext.MissionApplications.Where(app => app.UserId == userId).ToList();
            missionapp.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(missionapp);

            List<MissionInvite> invite = _cI_PlatformContext.MissionInvites.Where(app => app.FromUserId == userId || app.ToUserId == userId).ToList();
            invite.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(invite);

            List<MissionRating> rating = _cI_PlatformContext.MissionRatings.Where(app => app.UserId == userId).ToList();
            rating.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(rating);

            List<Story> story = _cI_PlatformContext.Stories.Where(app => app.UserId == userId).ToList();
            List<long> storyIds = new List<long>();
            story.ForEach(x => { x.DeletedAt = time; storyIds.Add(x.StoryId); });
            _cI_PlatformContext.UpdateRange(story);

            List<StoryMedium> storymed = new List<StoryMedium>();
            foreach (var i in storyIds)
            {
                storymed = _cI_PlatformContext.StoryMedia.Where(app => app.StoryId == i).ToList();
                storymed.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(storymed);
            }

            List<UserSkill> skills = _cI_PlatformContext.UserSkills.Where(app => app.UserId == userId).ToList();
            skills.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(skills);

            List<Timesheet> sheet = _cI_PlatformContext.Timesheets.Where(app => app.UserId == userId).ToList();
            sheet.ForEach(x => x.DeletedAt = time);
            _cI_PlatformContext.UpdateRange(sheet);

            if (user != null)
            {
                user.DeletedAt = time;
                user.Status = 0;
                _cI_PlatformContext.Users.Update(user);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            return false;
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
                var time = DateTime.Now;

                List<Comment> comments = _cI_PlatformContext.Comments.Where(comment => comment.MissionId == missionId).ToList();
                comments.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(comments);

                List<FavouriteMission> favMission = _cI_PlatformContext.FavouriteMissions.Where(fav => fav.MissionId == missionId).ToList();
                favMission.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(favMission);

                List<MissionApplication> missionapp = _cI_PlatformContext.MissionApplications.Where(app => app.MissionId == missionId).ToList();
                missionapp.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(missionapp);

                List<MissionInvite> invite = _cI_PlatformContext.MissionInvites.Where(app => app.MissionId == missionId).ToList();
                invite.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(invite);

                List<MissionRating> rating = _cI_PlatformContext.MissionRatings.Where(app => app.MissionId == missionId).ToList();
                rating.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(rating);

                List<Story> story = _cI_PlatformContext.Stories.Where(app => app.MissionId == missionId).ToList();
                List<long> storyIds = new List<long>();
                story.ForEach(x => { x.DeletedAt = time; storyIds.Add(x.StoryId); });

                _cI_PlatformContext.UpdateRange(story);

                List<StoryMedium> storymed = new List<StoryMedium>();
                foreach (var i in storyIds)
                {
                    storymed = _cI_PlatformContext.StoryMedia.Where(app => app.StoryId == i).ToList();
                    storymed.ForEach(x => x.DeletedAt = time);
                    _cI_PlatformContext.UpdateRange(storymed);
                }


                List<MissionSkill> skills = _cI_PlatformContext.MissionSkills.Where(app => app.MissionId == missionId).ToList();
                skills.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(skills);

                List<Timesheet> sheet = _cI_PlatformContext.Timesheets.Where(app => app.MissionId == missionId).ToList();
                sheet.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(sheet);

                List<MissionDocument> doc = _cI_PlatformContext.MissionDocuments.Where(doc => doc.MissionId == missionId).ToList();
                doc.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(doc);

                List<MissionMedium> media = _cI_PlatformContext.MissionMedia.Where(app => app.MissionId == missionId).ToList();
                media.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(media);

                if (mission.MissionType == "goal")
                {
                    GoalMission g = _cI_PlatformContext.GoalMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                    if (g != null)
                    {
                        g.DeletedAt = time;
                        _cI_PlatformContext.Update(g);
                    }
                }


                mission.DeletedAt = time;
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
            if(missionTheme != null)
            {
                missionTheme.DeletedAt = DateTime.Now;
                missionTheme.Status = 0;

                _cI_PlatformContext.MissionThemes.Update(missionTheme);
                _cI_PlatformContext.SaveChanges();

                return true;
            }
           return false;
        }


        //For Mission Skill Page on Admin
        public List<Skill> FetchSkillDetails()
        {
            return _cI_PlatformContext.Skills.Where(skill => skill.DeletedAt == null).ToList();
        }

        public Skill GetSkillDetail(long skillId)
        {
            return _cI_PlatformContext.Skills.Find(skillId);
        }

        public bool AddEditSkill(AdminMissionSkillsViewModel skillVm)
        {
            if(skillVm.skill.SkillId == 0)
            {
                Skill skill = new Skill();
                skill.SkillName = skillVm.skill.SkillName;
                skill.Status = skillVm.skill.Status;

                _cI_PlatformContext.Skills.Add(skill);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            else
            {
                Skill skill = _cI_PlatformContext.Skills.Find(skillVm.skill.SkillId);
                skill.SkillName = skillVm.skill.SkillName;
                skill.Status = skillVm.skill.Status;
                skill.UpdatedAt = DateTime.Now;

                _cI_PlatformContext.Skills.Update(skill);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
        }

        public bool DeleteSkill(long skillId)
        {
            Skill skill = _cI_PlatformContext.Skills.Find(skillId);
            if(skill != null)
            {
                List<UserSkill> userSkillList = _cI_PlatformContext.UserSkills.Where(skill => skill.SkillId == skillId).ToList();
                userSkillList.ForEach(x => x.DeletedAt = DateTime.Now);
                _cI_PlatformContext.UpdateRange(userSkillList);

                List<MissionSkill> missionSkillList = _cI_PlatformContext.MissionSkills.Where(skill => skill.SkillId == skillId).ToList();
                missionSkillList.ForEach(x => x.DeletedAt = DateTime.Now);
                _cI_PlatformContext.UpdateRange(missionSkillList);

                skill.DeletedAt = DateTime.Now;
                skill.Status = 0;
                _cI_PlatformContext.Update(skill);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            return false;
        }


        //For Mission Application Page
        public List<MissionApplication> FetchMissionApplication()
        {
            return _cI_PlatformContext.MissionApplications.Where(missionApplication => missionApplication.ApprovalStatus == "PENDING" && missionApplication.DeletedAt == null).ToList();
        }

        public List<User> FetchUser()
        {
            return _cI_PlatformContext.Users.Where(user => user.DeletedAt == null).ToList();
        }

        public bool ApproveRejectMissionApplication(int status,long approveId)
        {
            if(approveId == 0)
            {
                return false;
            }

            string stat = "";

            if(status == 0)
            {
                stat = "DECLINE";
            }
            else
            {
                stat = "APPROVE";
            }

            MissionApplication missionApplication = _cI_PlatformContext.MissionApplications.Find(approveId);
            missionApplication.ApprovalStatus = stat;
            missionApplication.UpdatedAt = DateTime.Now;

            _cI_PlatformContext.Update(missionApplication);
            _cI_PlatformContext.SaveChanges();
            return true;
        }



        //For Story Page
        public List<AdminStoryViewModel> FetchStoryList()
        {
            List<AdminStoryViewModel> storyAdminVM = new List<AdminStoryViewModel>();
            var query = _cI_PlatformContext.Stories.AsQueryable();
            query = query.Where(story => story.DeletedAt == null && story.Status == "PENDING");
            var queryable = query.Select(x => new AdminStoryViewModel()
            {
                storyDetails = x,
                userDetails = x.User,
                missionDetails = x.Mission,

            });
            storyAdminVM = queryable.ToList();
            return storyAdminVM;
        }

        public AdminStoryViewModel GetStoryDetails(long storyId)
        {
            AdminStoryViewModel vm = new AdminStoryViewModel();
            var query = _cI_PlatformContext.Stories.AsQueryable();
            query = query.Where(story => story.StoryId == storyId && story.DeletedAt == null);
            query = query.Where(story => story.Mission.DeletedAt == null);
            query = query.Where(story => story.User.DeletedAt == null);


            //vm.storyDetails = _context.Stories.Find(storyId);
            var queryselect = query.Select(story => new AdminStoryViewModel()
            {
                missionDetails = story.Mission,
                userDetails = story.User,
                storyDetails = story
            });
            vm = queryselect.FirstOrDefault();
            List<StoryMedium> m = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == storyId && x.Type != "video").ToList();
            if (m.Any())
            {
                m.ForEach(x => vm.storyimages.Add(x.Path));
            }

            return vm;
        }

        public bool StoryStatus(int status, long storyId)
        {
            if (storyId == 0)
            {
                return false;
            }

            string storyStatus = "";
            if (status == 0)
            {
                storyStatus = "DECLINED";
            }
            if (status == 1)
            {
                storyStatus = "PUBLISHED";
            }
            Story temp = _cI_PlatformContext.Stories.Find(storyId);
            temp.Status = storyStatus;
            _cI_PlatformContext.Update(temp);
            _cI_PlatformContext.SaveChanges();
            return true;
        }

        public bool DeleteStory(long storyId)
        {
            if (storyId != 0)
            {
                var time = DateTime.Now;
                Story story = _cI_PlatformContext.Stories.Find(storyId);
                story.DeletedAt = time;
                //story.Status = "DECLINED";
                List<StoryMedium> medium = _cI_PlatformContext.StoryMedia.Where(media => media.StoryId == storyId).ToList();
                medium.ForEach(x => x.DeletedAt = time);
                _cI_PlatformContext.UpdateRange(medium);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            return false;
        }

    }
}