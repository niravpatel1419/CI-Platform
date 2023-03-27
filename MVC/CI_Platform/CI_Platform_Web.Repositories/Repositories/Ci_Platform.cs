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
            vm.comments = _cI_PlatformContext.Comments.Where(x => x.MissionId == missionId).ToList();
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

        //For Shown The Story From DB
        public StoryListViewModel FetchStoryDetails()
        {
            StoryListViewModel v = new StoryListViewModel();

            v.users = _cI_PlatformContext.Users.FromSqlInterpolated($"select u.* from story s inner join [user] u on s.user_id=u.user_id").ToList();
            v.storyLists = _cI_PlatformContext.Stories.FromSqlInterpolated($"select s.* from story s inner join [user] u on s.user_id=u.user_id").ToList();
            v.StoryMedias = _cI_PlatformContext.StoryMedia.ToList();
            return v;
        }

        //For Save The Story In DB
        public bool SaveStrory(long userId, int missionId, string title, string stext, string date)
        {

            if (userId == 0)
            {
                return false;
            }
            Story s = new Story();
            s.UserId = userId;
            s.Title = title;
            s.MissionId = missionId;
            s.PublishedAt = DateTime.Parse(date);
            s.Description = stext;
            _cI_PlatformContext.Add(s);
            _cI_PlatformContext.SaveChanges();
            return true;

        }
    }

}
