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
    public class CI_Platform : ICI_Platform
    {
        private readonly CI_PlatformContext _cI_PlatformContext;

        public CI_Platform(CI_PlatformContext cI_PlatformContext)
        {
            _cI_PlatformContext = cI_PlatformContext;
        }

        //For User Login
        public User Login(User userLogin)
        {
            User user = new User();
            user = _cI_PlatformContext.Users.Where(x => x.Email == userLogin.Email && x.Password == userLogin.Password).FirstOrDefault();
            return user;
        }

        public Admin ValidateAdmin(User admin)
        {
            Admin user = new Admin();
            string email = admin.Email;
            string password= admin.Password;
            user = _cI_PlatformContext.Admins.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            return user;
        }


        //For User Register
        public bool IsRegister(User userRegister)
        {
            User u = _cI_PlatformContext.Users.Where(x => x.Email == userRegister.Email).FirstOrDefault();
            if (u == null)
            {
                User newUser = new User();
                newUser.Email = userRegister.Email;
                newUser.Password = userRegister.Password;
                newUser.FirstName = userRegister.FirstName;
                newUser.LastName = userRegister.LastName;
                newUser.PhoneNumber = userRegister.PhoneNumber;

                _cI_PlatformContext.Users.Add(newUser);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        //For Forgot Password
        public bool IsEmailExist(string email)
        {
            User u = _cI_PlatformContext.Users.Where(x => x.Email == email).FirstOrDefault();
            if (u == null)
            {
                return false;
            }
            else { return true; }
        }

        public void ForgetPassword(string email, string token)
        {

            // Store the token in the password resets table with the user's email
            PasswordReset r = new PasswordReset();
            r.Email = email;
            r.Token = token;

            _cI_PlatformContext.PasswordResets.Add(r);
            _cI_PlatformContext.SaveChanges();

        }


        //For Landing Page
        public MissionListViewModel DisplayMissions()
        {
            MissionListViewModel missions = new MissionListViewModel();
            missions.Missions = _cI_PlatformContext.Missions.Where(mission => mission.DeletedAt == null).ToList();
            missions.Cities = _cI_PlatformContext.Cities.Where(cities => cities.DeletedAt == null).ToList();
            missions.countries = _cI_PlatformContext.Countries.Where(countries => countries.DeletedAt == null).ToList();
            missions.MissionThemes = _cI_PlatformContext.MissionThemes.Where(missionthemes => missionthemes.DeletedAt == null).ToList();

            return missions;
        }

        public MissionListViewModel DisplayMissions(long? themeId, long? cityId, long? countryId)
        {
            MissionListViewModel missions = new MissionListViewModel();
            missions.Missions = _cI_PlatformContext.Missions.Where(mission => mission.ThemeId == themeId || mission.CityId == cityId || mission.CountryId == countryId).ToList();
            missions.Missions = missions.Missions.Where(x => x.DeletedAt == null).ToList();
            missions.Cities = _cI_PlatformContext.Cities.Where(cities => cities.DeletedAt == null).ToList();
            missions.countries = _cI_PlatformContext.Countries.Where(countries => countries.DeletedAt == null).ToList();
            missions.MissionThemes = _cI_PlatformContext.MissionThemes.Where(missionthemes => missionthemes.DeletedAt == null).ToList();
            missions.missionMedias = _cI_PlatformContext.MissionMedia.Where(missionmedia => missionmedia.DeletedAt == null).ToList();


            return missions;

        }


        //For Get The User Details
        public User GetUserDetails(long userId)
        {
            return _cI_PlatformContext.Users.Where(user => user.UserId == userId && user.DeletedAt == null).FirstOrDefault();
        }

        //For Get the Average Rating
        public int GetRating(int missionId)
        {
            List<MissionRating> temp = _cI_PlatformContext.MissionRatings.FromSqlInterpolated($"Select * from dbo.mission_rating where mission_id={missionId}").ToList();
            if (temp.Count == 0)
            {
                return 0;
            }
            else
            {
                return ((int)temp.Average(s => s.Rating));
            }

        }

        //For Add To Favourite Mission Check
        public bool IsFav(long userId, int missionId)
        {
            List<FavouriteMission> f = _cI_PlatformContext.FavouriteMissions.Where(favmission => favmission.UserId == userId && favmission.MissionId == missionId && favmission.DeletedAt == null).ToList();
            if (f.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //For Applied to Mission Check
        public string IsApplied(long userId, int missionId)
        {
            List<MissionApplication> f = _cI_PlatformContext.MissionApplications.Where(missionapplication => missionapplication.UserId == userId && missionapplication.MissionId == missionId && missionapplication.DeletedAt == null).ToList();
            if (f.Count == 0)
            {
                return "false";
            }
            else
            {
                return "true";

            }

        }

        //For The User Rating Check
        public int RatedValue(long userId, int missionId)
        {
            List<MissionRating> rr = _cI_PlatformContext.MissionRatings.Where(missionratings => missionratings.UserId == userId && missionratings.MissionId == missionId && missionratings.DeletedAt == null).ToList();
            if (rr.Count == 0)
            {
                return 0;
            }
            else
            {
                return rr[0].Rating;
            }

        }

        //For Get the Mission Info
        public Mission GetMissionDetails(int missionId)
        {
            Mission mission = _cI_PlatformContext.Missions.Where(mission => mission.MissionId == missionId && mission.DeletedAt == null).FirstOrDefault();
            return mission;
        }

        public VolunteeringMissionPageViewModel VolunteeringPageLoad(int missionId)
        {

            VolunteeringMissionPageViewModel vm = new VolunteeringMissionPageViewModel();
            vm.missions = GetMissionDetails(missionId);
            long themeid = vm.missions.ThemeId;
            long cityid = vm.missions.CityId;
            long countryid = vm.missions.CountryId;
            vm.relatedMissions = DisplayMissions(themeid, cityid, countryid);
            vm.users = _cI_PlatformContext.Users.ToList();
            vm.comments = _cI_PlatformContext.Comments.Where(x => x.MissionId == missionId).OrderByDescending(x => x.CreatedAt).ToList();
            vm.cities = _cI_PlatformContext.Cities.Where(x => x.CityId == cityid).FirstOrDefault();
            vm.countries = _cI_PlatformContext.Countries.Where(x => x.CountryId == countryid).FirstOrDefault();
            vm.themes = _cI_PlatformContext.MissionThemes.Where(x => x.MissionThemeId == themeid).FirstOrDefault();
            vm.goaltimestring = _cI_PlatformContext.GoalMissions.Where(x => x.MissionId == missionId).FirstOrDefault()?.GoalObjectiveText;
            vm.MissionSkills = _cI_PlatformContext.MissionSkills.Where(x => x.MissionId == missionId).ToList();
            vm.Skills = _cI_PlatformContext.Skills.ToList();
            vm.missionImages = _cI_PlatformContext.MissionMedia.Where(x => x.MissionId == missionId).ToList();
            vm.missionDocuments = _cI_PlatformContext.MissionDocuments.Where(X => X.MissionId == missionId).ToList();
            if (vm.goaltimestring == null)
            {
                vm.goaltimestring = "It is a time base mission";
            }

            return vm;
        }


        //For Add The Rating
        public bool AddRating(MissionRating r)
        {
            List<MissionRating> p = _cI_PlatformContext.MissionRatings.Where(missionratings => missionratings.UserId == r.UserId && missionratings.MissionId == r.MissionId && r.DeletedAt == null).ToList();
            if (p.Count == 0)
            {
                _cI_PlatformContext.MissionRatings.Add(r);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            else
            {

                p[0].Rating = r.Rating;

                _cI_PlatformContext.MissionRatings.Update(p[0]);
                _cI_PlatformContext.SaveChanges();

                return false;
            }
        }

        //For Add To Favourite Mission
        public int AddRemoveFav(long userId, int missionId)
        {
            IEnumerable<FavouriteMission> r = _cI_PlatformContext.FavouriteMissions.Where(favmission => favmission.UserId == userId && favmission.MissionId == missionId && favmission.DeletedAt == null).ToList();
            if (r.Count() == 0)
            {
                FavouriteMission f = new FavouriteMission();
                f.MissionId = missionId;
                f.UserId = userId;
                _cI_PlatformContext.FavouriteMissions.Add(f);
                _cI_PlatformContext.SaveChanges();

                return 1;
            }

            else
            {
                FavouriteMission fav = _cI_PlatformContext.FavouriteMissions.Where(favmission => favmission.UserId == userId && favmission.MissionId == missionId && favmission.DeletedAt == null ).FirstOrDefault();
                _cI_PlatformContext.Remove(fav);
                _cI_PlatformContext.SaveChanges();
                return 0;

            }

        }

        //For Apply to Mission
        public bool ApplyForMission(long userId, int missionId)
        {

            var m = _cI_PlatformContext.MissionApplications.Where(missionapplication => missionapplication.UserId == userId && missionapplication.MissionId == missionId && missionapplication.DeletedAt == null).FirstOrDefault();
            if (m == null)
            {
                MissionApplication a = new MissionApplication();
                a.UserId = userId;
                a.MissionId = missionId;
                a.AppliedAt = DateTime.Now;
                _cI_PlatformContext.MissionApplications.Add(a);
                _cI_PlatformContext.SaveChanges();

            }
          
            return true;
        }

        //For Adding the Commnets
        public bool AddComment(Comment c)
        {
            if (c != null)
            {
                _cI_PlatformContext.Comments.Add(c);
                _cI_PlatformContext.SaveChanges();
                return true;
            }
            return false;
        }

        //For Getting Recent Volunteers
        public List<User> GetRecentVol(int missionId)
        {
            List<User> i = _cI_PlatformContext.Users.FromSqlInterpolated($"select u.* from [user] as u inner join mission_application  m On u.user_id=m.user_id where m.mission_id={missionId}  ").ToList();
            return i;
        }


        //For Shown story from the database
        public IEnumerable<StoryListViewModel> FetchStoryDetails()
        {
            StoryListViewModel v = new StoryListViewModel();
            List<StoryListViewModel> vm = new List<StoryListViewModel>();
            v.users = _cI_PlatformContext.Users.Where(user => user.DeletedAt == null).ToList();
            v.storyLists = _cI_PlatformContext.Stories.Where(stories => stories.DeletedAt == null && stories.Status == "PUBLISHED").ToList();
            v.StoryMedias = _cI_PlatformContext.StoryMedia.Where(storymedia => storymedia.DeletedAt == null).ToList();

            List<Story> s = _cI_PlatformContext.Stories.Where(stories => stories.DeletedAt == null && stories.Status == "PUBLISHED").Include(x => x.StoryMedia).Include(x => x.User).ToList();
            return vm;
        }


        public IEnumerable<Story> GetStoryListData()
        {
            return _cI_PlatformContext.Stories.Where(stories => stories.DeletedAt == null && stories.Status == "PUBLISHED").Include(x => x.User).Where(x => x.Status == "PUBLISHED" && x.DeletedAt == null).ToList();
        }


        //For save the story details
        public bool SaveStrory(long userId, int missionId, string title, string stext, string date, string url, string status)
        {

            if (userId == 0 || missionId == 0)
            {
                return false;
            }
            Story story = _cI_PlatformContext.Stories.Where(stories => stories.UserId == userId && stories.Status == "DRAFT" && stories.MissionId == missionId && stories.DeletedAt == null).FirstOrDefault();
            if (story != null)
            {
                story.UserId = userId;
                story.Title = title;
                story.MissionId = missionId;
                story.PublishedAt = DateTime.Parse(date);
                story.Description = stext;
                story.Status = status;
                _cI_PlatformContext.Update(story);
                _cI_PlatformContext.SaveChanges();

                var id = _cI_PlatformContext.Stories.Where(stories => stories.UserId == userId && stories.MissionId == missionId && stories.PublishedAt == story.PublishedAt && stories.DeletedAt == null).OrderByDescending(x => x.CreatedAt).FirstOrDefault().StoryId;
                StoryMedium ma = _cI_PlatformContext.StoryMedia.Where(storymedia => storymedia.StoryId == id && storymedia.Type == "video" && storymedia.DeletedAt == null).FirstOrDefault();
                if (ma != null)
                {
                    if (url != null)
                    {
                        ma.StoryId = id;
                        ma.Path = url;
                        ma.Type = "video";
                        _cI_PlatformContext.Update(ma);
                        _cI_PlatformContext.SaveChanges();
                    }
                    else
                    {
                        _cI_PlatformContext.Remove(ma);
                        _cI_PlatformContext.SaveChanges();
                    }

                }

                else
                {
                    if (url != null)
                    {
                        StoryMedium m = new StoryMedium();
                        m.StoryId = id;
                        m.Type = "video";

                        m.Path = url;
                        _cI_PlatformContext.Add(m);
                        _cI_PlatformContext.SaveChanges();
                    }   

                }

            }


            else
            {
                Story s1 = new Story();
                s1.UserId = userId;
                s1.Title = title;
                s1.MissionId = missionId;
                var publishdate = DateTime.Parse(date);
                s1.PublishedAt = publishdate;
                s1.Description = stext;
                s1.Status = status;

                _cI_PlatformContext.Add(s1);
                _cI_PlatformContext.SaveChanges();

                //Story s1 = new Story();
                if (url != null)
                {
                    var id = _cI_PlatformContext.Stories.Where(stories => stories.UserId == userId && stories.Status == status && stories.PublishedAt == publishdate && stories.DeletedAt == null).FirstOrDefault().StoryId;

                    StoryMedium m = new StoryMedium();
                    m.StoryId = id;
                    m.Type = "video";
                    m.Path = url;
                    _cI_PlatformContext.Update(m);
                    _cI_PlatformContext.SaveChanges();
                }

            }

            return true;

        }


        //For Add the media in story
        public void AddStoryMedia(string mediaType, string mediaPath, long mid, long uid)
        {
            var storyId = _cI_PlatformContext.Stories.OrderByDescending(stories => stories.CreatedAt).Where(stories => (stories.MissionId == mid) && (stories.UserId == uid) && (stories.DeletedAt == null)).FirstOrDefault();
            var storymedia = _cI_PlatformContext.StoryMedia.Where(storymedia => storymedia.StoryId == storyId.StoryId && storymedia.DeletedAt == null).FirstOrDefault();

            StoryMedium sm = new StoryMedium();
            sm.StoryId = storyId.StoryId;
            sm.Type = mediaType;
            sm.Path = "/images/Story/" + mediaPath;
            _cI_PlatformContext.Add(sm);
            _cI_PlatformContext.SaveChanges();

        }


        //For Save the story details entered by the user
        public ShareStoryViewModel GetSavedStory(long userId,int missionId)
        {
            ShareStoryViewModel v = new ShareStoryViewModel();
            if (missionId == 0)
            {
                return v;
            }
            var i = _cI_PlatformContext.Stories.Where(stories => stories.UserId == userId && stories.Status == "DRAFT" && stories.MissionId == missionId && stories.DeletedAt == null).ToList();
            if (i.Count() > 0)
            {   
                v.stories = i.FirstOrDefault();
                v.media = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == v.stories.StoryId && x.DeletedAt == null).FirstOrDefault();
                v.url = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == v.stories.StoryId && x.Type == "video" && x.DeletedAt == null).FirstOrDefault()?.Path;
            }
            return v;
        }


        //For Story Detail Page
        public StoryDetailsVM GetStoryDetails(long storyId)
        {
            StoryDetailsVM v = new StoryDetailsVM();

            v.stories = _cI_PlatformContext.Stories.Where(x => x.StoryId == storyId && x.DeletedAt == null).Include(x => x.StoryMedia).Include(x => x.User).Include(x => x.StoryMedia).FirstOrDefault();
            v.users = _cI_PlatformContext.Users.ToList();
            Story s = _cI_PlatformContext.Stories.Where(x => x.StoryId == storyId && x.DeletedAt == null).FirstOrDefault();
            s.TotalViews = s.TotalViews + 1;
            _cI_PlatformContext.Update(s);
            _cI_PlatformContext.SaveChanges();
            return v;
        }


        //For Edit User Profile

        public bool UpdateUserDetails(User user, List<int> usersSkills)
        {
            User u = _cI_PlatformContext.Users.Where(user => user.UserId == user.UserId && user.DeletedAt == null).FirstOrDefault();
            
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.EmployeeId = user.EmployeeId;
            u.Manager = user.Manager;
            u.Title = user.Title;                                                                   
            u.Department = user.Department;
            u.ProfileText = user.ProfileText;
            u.WhyIVolunteer = user.WhyIVolunteer;
            u.CountryId = user.CountryId;
            u.CityId = user.CityId;
            u.Availability = user.Availability;
            u.LinkedInUrl = user.LinkedInUrl;
            u.UpdatedAt = DateTime.Now;
            if (user.Avatar != null)
            {
                u.Avatar = user.Avatar;
            }
            _cI_PlatformContext.Users.Update(u);
            _cI_PlatformContext.SaveChanges();

            var a = _cI_PlatformContext.Database.ExecuteSqlRaw($"delete from user_skill where user_id={user.UserId}");
            _cI_PlatformContext.SaveChanges();

            List<UserSkill> userSkillsList = new List<UserSkill>();
            foreach (var id in usersSkills)
            {
                userSkillsList.Add(new UserSkill { SkillId = id, UserId = user.UserId });
            }
            _cI_PlatformContext.UserSkills.AddRange(userSkillsList);
            _cI_PlatformContext.SaveChanges();

            return true;
            
        }


        //For Change The User Password In The User Edit Profile Section

        public int ChangeUserPassword(UserDetailsViewModel u)
        {
            User user = _cI_PlatformContext.Users.Where(user => user.UserId == u.users.UserId && user.DeletedAt == null).FirstOrDefault();

            if(user.Password != u.oldPassword)
            {
                return 0;
            }
            else
            {
                user.Password = u.newPassword;
                _cI_PlatformContext.Update(user);
                _cI_PlatformContext.SaveChanges();

                return 1;
            }
        }

        //For Country in user edit profile page
        public List<Country> GetCountryList()
        {
            return _cI_PlatformContext.Countries.ToList();
        }

        //For Add skills in user edit profile page
        public List<Skill> GetAllSkills()
        {
            return _cI_PlatformContext.Skills.ToList();
        }

        public List<UserSkill> GetUsersSkills(long userId)
        {
            return _cI_PlatformContext.UserSkills.Where(userskills => userskills.UserId == userId && userskills.DeletedAt == null).ToList();
        }

        public volunteeringTimeSheetViewModel GetVolunteerTimeDetails(long userId)
        {
            volunteeringTimeSheetViewModel vol = new volunteeringTimeSheetViewModel();
            List<MissionApplication> goalMission = _cI_PlatformContext.MissionApplications.Where(missionapplication => missionapplication.UserId == userId && missionapplication.Mission.MissionType == "goal" && missionapplication.DeletedAt == null).ToList();
            List<MissionApplication> timeMission = _cI_PlatformContext.MissionApplications.Where(missionapplication => missionapplication.UserId == userId && missionapplication.Mission.MissionType == "time" && missionapplication.DeletedAt == null).ToList();
            vol.timeMissions = timeMission;
            vol.goalMissions = goalMission;
            vol.allMissions = _cI_PlatformContext.Missions.ToList();
            vol.goalTimesheetList = _cI_PlatformContext.Timesheets.Where(timesheets => timesheets.UserId == userId && timesheets.Mission.MissionType == "goal" && timesheets.DeletedAt == null).ToList();
            vol.timeTimesheetList = _cI_PlatformContext.Timesheets.Where(timesheets => timesheets.UserId == userId && timesheets.Mission.MissionType == "time" && timesheets.DeletedAt == null).ToList();
            return vol;
        }

        public bool AddTimeSheetEntry(long userId, volunteeringTimeSheetViewModel vm)
        {

            Timesheet t = new Timesheet();
            if (vm.timesheetPrimary != 0)
            {
                t = _cI_PlatformContext.Timesheets.Where(timesheets => timesheets.TimesheetId == vm.timesheetPrimary && timesheets.DeletedAt == null).FirstOrDefault();
            }
            t.UserId = userId;
            t.MissionId = vm.missionId;
            t.DateVolunteered = vm.date;
            t.Notes = vm.message;

            if (vm.missionType == "time")
            {
                t.Time = TimeSpan.Parse(vm.hours.ToString() + ":" + vm.minutes.ToString() + ":00");
            }
            if (vm.missionType == "goal")
            {
                t.Action = vm.action;
            }
            if (vm.timesheetPrimary != 0)
            {
                _cI_PlatformContext.Timesheets.Update(t);
                _cI_PlatformContext.SaveChanges();
                return true;

            }

            _cI_PlatformContext.Timesheets.Add(t);
            _cI_PlatformContext.SaveChanges();
            return true;
        }

        public bool DeleteTimesheetRecord(int timesheetId)
        {
            _cI_PlatformContext.Database.ExecuteSqlRaw($"delete from timesheet where timesheet_id={timesheetId}");
            return true;
        }

        public Timesheet EditTimesheetRecord(long timesheetId)
        {
            Timesheet t = _cI_PlatformContext.Timesheets.Find(timesheetId);
            return t;
        }


        /*public Admin ValidateAdmin(User admin)
        {
            Admin ad = _cI_PlatformContext.Admins.Where(admi => admi.Email == admin.Email && admi.Password == admin.Password && admi.DeletedAt == null).FirstOrDefault();
            return ad;
        }*/

        public int GetProgress(long missionId)
        {
            if (missionId == 0 || missionId < 0)
            {
                return -2;
            }

            Mission m = _cI_PlatformContext.Missions.Where(mission => mission.MissionId == missionId && mission.DeletedAt == null).FirstOrDefault();
            if (m == null)
            {
                return -2;
            }
            if (m.MissionType == "time")
            {
                return -1;
            }


            int done = (int)_cI_PlatformContext.Timesheets.Where(x => x.MissionId == missionId && x.DeletedAt == null).Select(x => x.Action).ToList().Sum();
            int total = _cI_PlatformContext.GoalMissions.Where(x => x.MissionId == missionId && x.DeletedAt == null).Select(x => x.GoalValue).FirstOrDefault();
            return (int)(((float)done / (float)total) * 100);

        }

        public int GetSeatLeft(long missionId)
        {
            if (missionId == 0 || missionId < 0)
            {
                return -2;
            }
            int applied = _cI_PlatformContext.MissionApplications.Count(app => app.MissionId == missionId && app.DeletedAt == null && app.ApprovalStatus == "APPROVE");
            string s = _cI_PlatformContext.Missions.Where(mission => mission.DeletedAt == null && mission.MissionId == missionId).FirstOrDefault().Seatleft;
            int left = int.Parse(s)-applied;
            return left;
        }





    }

}
    