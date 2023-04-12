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
            missions.Missions = _cI_PlatformContext.Missions.ToList();
            missions.Cities = _cI_PlatformContext.Cities.ToList();
            missions.countries = _cI_PlatformContext.Countries.ToList();
            missions.MissionThemes = _cI_PlatformContext.MissionThemes.ToList();

            return missions;
        }

        public MissionListViewModel DisplayMissions(long? themeId, long? cityId, long? countryId)
        {
            MissionListViewModel missions = new MissionListViewModel();
            missions.Missions = _cI_PlatformContext.Missions.Where(x => x.ThemeId == themeId || x.CityId == cityId || x.CountryId == countryId).ToList();
            missions.Cities = _cI_PlatformContext.Cities.ToList();
            missions.countries = _cI_PlatformContext.Countries.ToList();
            missions.MissionThemes = _cI_PlatformContext.MissionThemes.ToList();



            return missions;

        }


        //For Get The User Details
        public User GetUserDetails(long userId)
        {
            return _cI_PlatformContext.Users.Where(x => x.UserId == userId).FirstOrDefault();
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
            List<FavouriteMission> f = _cI_PlatformContext.FavouriteMissions.Where(f => f.UserId == userId && f.MissionId == missionId).ToList();
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
            List<MissionApplication> f = _cI_PlatformContext.MissionApplications.Where(f => f.UserId == userId && f.MissionId == missionId).ToList();
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
            List<MissionRating> rr = _cI_PlatformContext.MissionRatings.Where(x => x.UserId == userId && x.MissionId == missionId).ToList();
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
            Mission mission = _cI_PlatformContext.Missions.Where(x => x.MissionId == missionId).FirstOrDefault();
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

            if (vm.goaltimestring == null)
            {
                vm.goaltimestring = "It is a time base mission";
            }

            return vm;
        }


        //For Add The Rating
        public bool AddRating(MissionRating r)
        {
            List<MissionRating> p = _cI_PlatformContext.MissionRatings.Where(x => x.UserId == r.UserId && x.MissionId == r.MissionId).ToList();
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
            IEnumerable<FavouriteMission> r = _cI_PlatformContext.FavouriteMissions.Where(x => x.UserId == userId && x.MissionId == missionId).ToList();
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
                FavouriteMission fav = _cI_PlatformContext.FavouriteMissions.Where(x => x.UserId == userId && x.MissionId == missionId).FirstOrDefault();
                _cI_PlatformContext.Remove(fav);
                _cI_PlatformContext.SaveChanges();
                return 0;

            }

        }

        //For Apply to Mission
        public bool ApplyForMission(long userId, int missionId)
        {

            var m = _cI_PlatformContext.MissionApplications.Where(x => x.UserId == userId && x.MissionId == missionId).FirstOrDefault();
            if (m == null)
            {
                MissionApplication a = new MissionApplication();
                a.UserId = userId;
                a.MissionId = missionId;
                a.AppliedAt = DateTime.Now;
                _cI_PlatformContext.MissionApplications.Add(a);
                _cI_PlatformContext.SaveChanges();

            }
            else
            {

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
            //  return _context.MissionApplications.Where(x => x.MissionId == missionId).ToList();
            List<User> i = _cI_PlatformContext.Users.FromSqlInterpolated($"select u.* from [user] as u inner join mission_application  m On u.user_id=m.user_id where m.mission_id={missionId}  ").ToList();
            return i;
        }

        //For Shown story from the database
        public IEnumerable<StoryListViewModel> FetchStoryDetails()
        {
            StoryListViewModel v = new StoryListViewModel();
            List<StoryListViewModel> vm = new List<StoryListViewModel>();
            // v.users = _context.Users.FromSql($"select u.* from story s inner join [user] u on s.user_id=u.user_id").ToList();
            //  v.storyLists= _context.Stories.FromSql($"select s.* from story s inner join [user] u on s.user_id=u.user_id").ToList();
            // v.StoryMedias= _context.StoryMedia.FromSql($"select Sm.* from story S left join story_MEDIA SM ON S.STORY_ID=SM.STORY_ID").ToList();
            v.users = _cI_PlatformContext.Users.ToList();
            v.storyLists = _cI_PlatformContext.Stories.ToList();
            v.StoryMedias = _cI_PlatformContext.StoryMedia.ToList();
            //var id = from e1 in v.users
            //         join e2 in v.storyLists on e1.UserId equals e2.UserId
            //         join
            //         e3 in v.StoryMedias on e2.StoryId equals e3.StoryId
            //         select new StoryListViewModel
            //         {
            //             user = e1,
            //             storyList = e2,
            //             StoryMedia = e3,
            //         };
            //var iid = from e1 in v.storyLists
            //          join e2 in v.StoryMedias  on e1.StoryId equals e2.StoryId into eg
            //          from e2 in eg.DefaultIfEmpty()
            //          join e3 in v.users on e1.UserId equals e3.UserId
            //          select new StoryListViewModel
            //          {
            //              StoryMedia= e2,
            //              storyList=e1,
            //              user=e3
            //          };
            List<Story> s = _cI_PlatformContext.Stories.Include(x => x.StoryMedia).Include(x => x.User).ToList();
            return vm;
        }


        public IEnumerable<Story> GetStoryListData()
        {
            //return _cI_PlatformContext.Stories.Include(x => x.StoryMedia).Include(x => x.User).Where(x => x.Status != "DRAFT").ToList();
            return _cI_PlatformContext.Stories.Include(x => x.User).Where(x => x.Status == "DRAFT").ToList();
        }


        //For save the story details
        public bool SaveStrory(long userId, int missionId, string title, string stext, string date, string url, string status)
        {

            if (userId == 0 || missionId == 0)
            {
                return false;
            }
            Story s = _cI_PlatformContext.Stories.Where(x => x.UserId == userId && x.Status == "DRAFT" && x.MissionId == missionId).FirstOrDefault();
            if (s != null)
            {
                s.UserId = userId;
                s.Title = title;
                s.MissionId = missionId;
                s.PublishedAt = DateTime.Parse(date);
                s.Description = stext;
                s.Status = status;
                _cI_PlatformContext.Update(s);
                _cI_PlatformContext.SaveChanges();

                var id = _cI_PlatformContext.Stories.Where(x => x.UserId == userId && x.MissionId == missionId && x.PublishedAt == s.PublishedAt).OrderByDescending(x => x.CreatedAt).FirstOrDefault().StoryId;
                StoryMedium ma = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == id && x.Type == "video").FirstOrDefault();
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
                    var id = _cI_PlatformContext.Stories.Where(x => x.UserId == userId && x.Status == status && x.PublishedAt == publishdate).FirstOrDefault().StoryId;

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
            var storyId = _cI_PlatformContext.Stories.OrderByDescending(e => e.CreatedAt).Where(e => (e.MissionId == mid) && (e.UserId == uid)).FirstOrDefault();
            var storymedia = _cI_PlatformContext.StoryMedia.Where(e => e.StoryId == storyId.StoryId).FirstOrDefault();

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
            var i = _cI_PlatformContext.Stories.Where(x => x.UserId == userId && x.Status == "DRAFT" && x.MissionId == missionId).ToList();
            if (i.Count() > 0)
            {   
                v.stories = i.FirstOrDefault();
                v.media = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == v.stories.StoryId).FirstOrDefault();
                v.url = _cI_PlatformContext.StoryMedia.Where(x => x.StoryId == v.stories.StoryId && x.Type == "video").FirstOrDefault()?.Path;
            }
            return v;
        }


        //For Story Detail Page
        public StoryDetailsVM GetStoryDetails(long storyId)
        {
            StoryDetailsVM v = new StoryDetailsVM();

            v.stories = _cI_PlatformContext.Stories.Where(x => x.StoryId == storyId).Include(x => x.StoryMedia).Include(x => x.User).Include(x => x.StoryMedia).FirstOrDefault();
            v.users = _cI_PlatformContext.Users.ToList();
            Story s = _cI_PlatformContext.Stories.Where(x => x.StoryId == storyId).FirstOrDefault();
            s.TotalViews = s.TotalViews + 1;
            _cI_PlatformContext.Update(s);
            _cI_PlatformContext.SaveChanges();
            return v;
        }


        //For Edit User Profile

        public bool UpdateUserDetails(User u, List<int> usersSkills)
        {
            User temp = _cI_PlatformContext.Users.Where(x => x.UserId == u.UserId).FirstOrDefault();
            
            temp.FirstName = u.FirstName;
            temp.LastName = u.LastName;
            temp.EmployeeId = u.EmployeeId;
            temp.Manager = u.Manager;
            temp.Title = u.Title;                                                                   
            temp.Department = u.Department;
            temp.ProfileText = u.ProfileText;
            temp.WhyIVolunteer = u.WhyIVolunteer;
            temp.CountryId = u.CountryId;
            temp.CityId = u.CityId;
            temp.Availability = u.Availability;
            temp.LinkedInUrl = u.LinkedInUrl;
            temp.UpdatedAt = DateTime.Now;
            if (u.Avatar != null)
            {
                temp.Avatar = u.Avatar;
            }
            _cI_PlatformContext.Users.Update(temp);
            _cI_PlatformContext.SaveChanges();

            var a = _cI_PlatformContext.Database.ExecuteSqlRaw($"delete from user_skill where user_id={u.UserId}");
            _cI_PlatformContext.SaveChanges();

            List<UserSkill> userSkillsList = new List<UserSkill>();
            foreach (var id in usersSkills)
            {
                userSkillsList.Add(new UserSkill { SkillId = id, UserId = u.UserId });
            }
            _cI_PlatformContext.UserSkills.AddRange(userSkillsList);
            _cI_PlatformContext.SaveChanges();

            return true;
            
        }

        //For Change The User Password In The User Edit Profile Section

        public bool changeUserPassword(UserDetailsViewModel u)
        {
            User user = _cI_PlatformContext.Users.Where(x => x.UserId == u.users.UserId).FirstOrDefault();

            if(user.Password != u.oldPassword)
            {
                return false;
            }
            else
            {
                user.Password = u.newPassword;
                _cI_PlatformContext.Update(user);
                _cI_PlatformContext.SaveChanges();

                return true;
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
            return _cI_PlatformContext.UserSkills.Where(x => x.UserId == userId).ToList();
        }

        public volunteeringTimeSheetViewModel GetVolunteerTimeDetails(long userId)
        {
            volunteeringTimeSheetViewModel vol = new volunteeringTimeSheetViewModel();
            List<MissionApplication> goalMission = _cI_PlatformContext.MissionApplications.Where(x => x.UserId == userId && x.Mission.MissionType == "goal").ToList();
            List<MissionApplication> timeMission = _cI_PlatformContext.MissionApplications.Where(x => x.UserId == userId && x.Mission.MissionType == "time").ToList();
            vol.timeMissions = timeMission;
            vol.goalMissions = goalMission;
            vol.allMissions = _cI_PlatformContext.Missions.ToList();
            vol.goalTimesheetList = _cI_PlatformContext.Timesheets.Where(x => x.UserId == userId && x.Mission.MissionType == "goal").ToList();
            vol.timeTimesheetList = _cI_PlatformContext.Timesheets.Where(x => x.UserId == userId && x.Mission.MissionType == "time").ToList();
            return vol;
        }

        public bool AddTimeSheetEntry(long userId, volunteeringTimeSheetViewModel vm)
        {

            Timesheet t = new Timesheet();
            if (vm.timesheetPrimary != 0)
            {
                t = _cI_PlatformContext.Timesheets.Where(x => x.TimesheetId == vm.timesheetPrimary).FirstOrDefault();
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
    }

}
    