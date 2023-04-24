using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Web;
using NuGet.Common;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using CI_Platform_Web.Repositories.Interface;
using CI_Platform_Web.Entities.ViewModel;
using Microsoft.Extensions.Hosting.Internal;

namespace CI_Platform_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CI_PlatformContext _cI_PlatformContext;
        private readonly ICI_Platform _iCiPlat;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        int i = 0, i1 = 0, j = 0, j1 = 0, k = 0, k1 = 0;

        public HomeController(ILogger<HomeController> logger, CI_PlatformContext cI_PlatformContext, ICI_Platform iCiPlat, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _cI_PlatformContext = cI_PlatformContext;
            _iCiPlat = iCiPlat;
            _hostingEnvironment = hostingEnvironment;
        }


        //For Logout

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }



        //For LandingPage

        public IActionResult home(string searchQuery, long id, int? pageIndex, int sortId, string[] countryList, string[] cityList, string[] themeList)
        {

            if (sortId != null)
            {
                ViewBag.sortIdPass = sortId;
            }
            else
            {
                ViewBag.sortIdPass = 0;
            }

            var identity = User.Identity as ClaimsIdentity;
            var userEmail = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);

            ViewBag.FirstName = userName;
            ViewBag.UserId = userId;


            //For shown the Country list in the Dropdown
            List<Country> Countries = _cI_PlatformContext.Countries.ToList();
            ViewBag.Country = Countries;

            List<Country> countryElements = _cI_PlatformContext.Countries.ToList();

            //For shown the City list in the Dropdown
            List<City> Cities = _cI_PlatformContext.Cities.ToList();
            ViewBag.City = Cities;

            //For shown the Theme list in  the Dropdown
            List<MissionTheme> Themes = _cI_PlatformContext.MissionThemes.ToList();
            ViewBag.MissionThemes = Themes;

            //For shown the Skills list in the Dropdown
            List<Skill> skills = _cI_PlatformContext.Skills.ToList();
            ViewBag.Skills = skills;

            //For Recommend to a Co-Worker
            List<User> user = _cI_PlatformContext.Users.ToList();
            ViewBag.User = user;

            List<GoalMission> goalMission = _cI_PlatformContext.GoalMissions.ToList();
            ViewBag.goalMission = goalMission;



            //For Shown the Mission Detalis On the Card
            MissionListViewModel missionlist = new MissionListViewModel();
            missionlist = _iCiPlat.DisplayMissions();


            //For Filtering The Mission

            var newMissions = missionlist.Missions;
            List<Mission> filteredMission = new List<Mission>();
            List<Mission> filteredMission1 = new List<Mission>();
            List<Mission> filteredMission2 = new List<Mission>();

            if (countryList.Count() > 0 || themeList.Count() > 0 || cityList.Count() > 0)
            {
                long id2 = 0;
                List<Mission> temp = new List<Mission>();
                if (countryList.Count() > 0)
                {
                    foreach (var country in countryList)
                    {
                        id2 = missionlist.countries.Where(x => x.Name == country).FirstOrDefault().CountryId;
                        temp = missionlist.Missions.Where(x => x.CountryId == id2).ToList();
                        filteredMission.AddRange(temp);
                    }
                    missionlist.Missions = filteredMission;
                }
                if (cityList.Count() > 0)
                {
                    foreach (var city in cityList)
                    {
                        id2 = missionlist.Cities.Where(x => x.Name == city).FirstOrDefault().CityId;
                        temp = missionlist.Missions.Where(x => x.CityId == id2).ToList();
                        filteredMission1.AddRange(temp);
                    }
                    missionlist.Missions = filteredMission1;

                }
                if (themeList.Count() > 0)
                {

                    foreach (var theme in themeList)
                    {
                        id2 = missionlist.MissionThemes.Where(x => x.Title == theme).FirstOrDefault().MissionThemeId;
                        temp = missionlist.Missions.Where(x => x.ThemeId == id2).ToList();
                        filteredMission2.AddRange(temp);
                    }

                    missionlist.Missions = filteredMission2;
                }


            }


            //For Sort By Feature

            switch (sortId)
            {
                case 1:
                    missionlist.Missions = missionlist.Missions.OrderByDescending(m => m.StartDate).ToList();
                    ViewBag.sortby = "Newest";
                    break;
                case 2:
                    missionlist.Missions = missionlist.Missions.OrderBy(m => m.StartDate).ToList();
                    ViewBag.sortby = "Oldest";
                    break;
                case 3:
                    missionlist.Missions = missionlist.Missions.OrderBy(m => int.Parse(m.Seatleft)).ToList();
                    break;
                case 4:
                    missionlist.Missions = missionlist.Missions.OrderBy(m => m.EndDate).ToList();
                    break;
            }
            //    case "Highest seats":
            //        mission = mission.OrderByDescending(m => int.Parse(m.Availability)).ToList();
            //        break;
            //    case "Registration deadline":
            //        mission = mission.OrderBy(m => m.EndDate).ToList();
            //        break;

            //}



            //for search the mission

            if (searchQuery != null)
            {
                string s = searchQuery.ToLower();
                missionlist.Missions = missionlist.Missions.Where(m => m.Title.ToLower().Contains(s)).ToList();

                ViewBag.searchQuery = searchQuery;
                if (missionlist.Missions.Count() == 0)
                {
                    ViewData["nomission"] = "No Mission Found";
                }

            }

            //For the Pagination

            int pageSize = 6;
            int skip = (pageIndex ?? 0) * pageSize;

            var allmisions = missionlist.Missions;
            var Missions = allmisions.Skip(skip).Take(pageSize).ToList();

            int totalMissions = missionlist.Missions.Count();
            missionlist.Missions = Missions;

            ViewBag.TotalMission = totalMissions;

            ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            ViewBag.CurrentPage = pageIndex ?? 0;

            List<int> missionratings = new List<int>();
            List<MissionMedium> m = new List<MissionMedium>();
            List<bool> b = new List<bool>();
            List<string> c = new List<string>();
          /*  List<int> progress = new List<int>();
            List<int> seatLeft = new List<int>();*/

            foreach (var i in missionlist.Missions)
            {
                missionratings.Add(_iCiPlat.GetRating((int)i.MissionId));
                /* m.Add(missionlist.missionMedias.Where(x => x.MissionId == i.MissionId).FirstOrDefault());*/
                b.Add(_iCiPlat.IsFav(userId, (int)i.MissionId));
                c.Add(_iCiPlat.IsApplied(userId, (int)i.MissionId));
                /*progress.Add(_iciplat.GetProgress(i.MissionId));
                seatLeft.Add(_iciplat.GetSeatLeft(i.MissionId));*/
            }
            missionlist.MissionRatingss = missionratings;
            missionlist.missionMedias = m.ToArray();
            missionlist.FavMission = b;
            missionlist.MissionApplicationlist = c;
            /*missionlist.progress = progress;
            missionlist.seatleft = seatLeft;*/

            return View(missionlist);

        }


        //For the no mission found

        public IActionResult noMissionFound()
        {
            return View();
        }


        //For the volunteering mission 

        public IActionResult volunteeringMission(int missID)
        {
            VolunteeringMissionPageViewModel vm = _iCiPlat.VolunteeringPageLoad(missID);

            //USer Details From Claim Authentication
            var identity = User.Identity as ClaimsIdentity;
            var userEmail = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);
            ViewBag.UserId = userId;
            ViewBag.FirstName = userName;

            //To store the mission id
            ViewBag.mid = missID;

            //For Recent Volunteers
            vm.users = _iCiPlat.GetRecentVol(missID);

            //For Applied to Mission Or Not
            vm.Applied = _iCiPlat.IsApplied(userId, missID);

            //For Add to Favourite Mission

            if (!_iCiPlat.IsFav(userId, missID))
            {
                ViewBag.favmission = "Set";
            }


            //For Recommend to a Co-Worker
            List<User> user = _cI_PlatformContext.Users.ToList();
            ViewBag.User = user;


            //For Mission Rating

            int rateCount = _iCiPlat.RatedValue(userId, missID);
            if (rateCount != 0)
            {
                ViewBag.rate = rateCount;
            }


            //For Comment
            if (vm.comments.Count() == 0)
            {
                ViewData["NoComment"] = "NO ONE COMMENTED YET BE FIRST ONE TO COMMENT";
            }


            //For Shown the Average Rating of the Mission
            int avgRate = _iCiPlat.GetRating(missID);
            if (avgRate != 0)
            {
                ViewBag.Ratingcount = avgRate;
            }

            //For the releated mission


            if (vm.relatedMissions.Missions.Count() == 0)
            {
                ViewData["NoRelatedMission"] = "No Related Mission Available";
            }

            List<int> ab = new List<int>(3);
            List<Mission> temp = vm.relatedMissions.Missions.Take(3).ToList();
            List<MissionMedium> m = new List<MissionMedium>();
            List<bool> b = new List<bool>();
            List<string> c = new List<string>();
            foreach (var i in temp)
            {
                var a = _iCiPlat.GetRating((int)i.MissionId);
                ab.Add(a);
                /*  var ji = vm.relatedMissions.missionMedias.Where(x => x.MissionId == i.MissionId).FirstOrDefault();
                  m.Add(ji);*/
                b.Add(_iCiPlat.IsFav(userId, (int)i.MissionId));
                c.Add(_iCiPlat.IsApplied(userId, (int)i.MissionId));
            }
            vm.relatedMissions.Missions = temp;
            vm.relatedMissions.MissionRatingss = ab;
            /* vm.relatedMissions.missionMedias = m.ToArray();*/
            vm.relatedMissions.FavMission = b;
            vm.relatedMissions.MissionApplicationlist = c;
            return View(vm);

        }



        //For Add to Favourite Mission
        public int addToFav(int id, int mid)
        {

            int fav1 = _iCiPlat.AddRemoveFav(id, mid);

            return fav1;

        }


        //For User Rating in Volunteering Mission Page
        public int Ratee(int uid, int mid, int rating)
        {
            MissionRating r = new MissionRating();
            r.Rating = rating;
            r.UserId = uid;
            r.MissionId = mid;
            bool temp = _iCiPlat.AddRating(r);
            if (temp)
            {
                return 1;
            }
            else { return 3; }

        }


        //For Apply to Mission
        public bool Apply(int missionId)
        {
            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity?.FindFirst(ClaimTypes.Sid)?.Value);
            return _iCiPlat.ApplyForMission(userId, missionId);

        }


        //For the User Comment

        [HttpPost]
        public IActionResult AddComment(int missionId, string text)
        {
            var identity = User.Identity as ClaimsIdentity;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);

            //string s = Request.Form["comment"].ToString();
            // long l = long.Parse(Request.Form["mid"].ToString());

            if (text == "")
            {
                return RedirectToAction("volunteeringMission", "Home", new { missID = missionId });
            }
            else
            {

                Comment comment = new Comment();
                comment.UserId = userId;


                comment.Commenttext = text;
                comment.MissionId = missionId;
                bool t = _iCiPlat.AddComment(comment);


                return RedirectToAction("volunteeringMission", "Home", new { missID = missionId });
            }

        }


        //For reccommendation to co-worker

        public bool recommendEmail(int sID, int m)
        {
            string user = _cI_PlatformContext.Users.Where(x => x.UserId == sID).FirstOrDefault().Email;
            ViewBag.User = user;

            var invitationLink = Url.Action("volunteeringMission", "Home", new { missID = m }, Request.Scheme);

            var fromAddress = new MailAddress("computerengineermeet@gmail.com", "CI_Platform");
            var toAddress = new MailAddress(user);
            var subject = "Someone Recommended you to join Mission";
            var body = $"Hi,Nirav Patel here, <br/> Someone has recommended you to join this mission...<br /><br />Please click on the following link to see mission details:<br /><br /><a href='{invitationLink}'>{invitationLink}</a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("computerengineermeet@gmail.com", "kknqzbwyupzddahv"),
                EnableSsl = true
            };
            smtpClient.Send(message);

            return true;
            // return RedirectToAction("volunteeringMission", "Home", new { missID = m });

        }



        //For StoryListing Page
        public IActionResult storyListingPage()
        {

            //User Details From Claim Authentication
            var identity = User.Identity as ClaimsIdentity;
            var userEmail = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);
            ViewBag.UserId = userId;
            ViewBag.FirstName = userName;

            //IEnumerable<StoryListViewModel> c;
            // c = _iCiPlat.FetchStoryDetails();
            //return View(c);

            var cii = _iCiPlat.GetStoryListData();
            return View(cii);
        }


        //For Share Your Story Page
        public IActionResult ShareStory(int missionId)
        {
            //User Details From Claim Authentication
            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);
            var userName = identity?.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.UserId = userId;
            ViewBag.FirstName = userName;

            ShareStoryViewModel v = _iCiPlat.GetSavedStory(userId, missionId);
            v.missionlist = _iCiPlat.DisplayMissions();
            return View(v);
        }


        [HttpPost]
        public IActionResult ShareStory(ShareStoryViewModel v, string button)
        {
            if (button == "CANCEL")
            {
                return RedirectToAction("storyListingPage", "Home");
            }

            string d = Request.Form["editor1"].ToString();
            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);

            if (v.missionId == 0)
            {
                ViewBag.Mission = "Select a valid mission";
                ShareStoryViewModel vm = _iCiPlat.GetSavedStory(userId, v.missionId);
                vm.missionlist = _iCiPlat.DisplayMissions();
                return View(vm);
            }

            if (v.stories.Title == null)
            {
                ViewBag.Title = "Please enter a valid title";
                ShareStoryViewModel vm = _iCiPlat.GetSavedStory(userId, v.missionId);
                vm.missionlist = _iCiPlat.DisplayMissions();
                return View(vm);
            }

            if (v.stories.PublishedAt == null)
            {
                ViewBag.PublishedAt = "Please choose a published date";
                ShareStoryViewModel vm = _iCiPlat.GetSavedStory(userId, v.missionId);
                vm.missionlist = _iCiPlat.DisplayMissions();
                return View(vm);
            }

            if (d == "")
            {
                ViewBag.description = "Please enter a valid description";
                ShareStoryViewModel vm = _iCiPlat.GetSavedStory(userId, v.missionId);
                vm.missionlist = _iCiPlat.DisplayMissions();
                return View(vm);
            }

            int missionId = v.missionId;
            string t = v.stories.Title;
            string url = v.url;
            string date = v.stories.PublishedAt.ToString();
            bool b = _iCiPlat.SaveStrory(userId, missionId, t, d, date, url, button);


            if (v.attachment != null)
            {
                foreach (var i in v.attachment)
                {
                    if (i != null)
                    {
                        string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images//Story");
                        string filename = Guid.NewGuid().ToString() + "_" + i.FileName;
                        string filepath = Path.Combine(UploadsFolder, filename);

                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            i.CopyTo(fileStream);
                        }
                        _iCiPlat.AddStoryMedia(Convert.ToString(i.ContentType.Split("/")[1]), Convert.ToString(filename), missionId, userId);
                    }
                }
            }
            return RedirectToAction("storyListingPage", "Home");
        }


        //For Story Detail

        public IActionResult StoryDetail(long storyId)
        {
            var identity = User.Identity as ClaimsIdentity;
            string fName = identity.FindFirst(ClaimTypes.Name).Value;
            ViewBag.FirstName = fName;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);

            StoryDetailsVM v = _iCiPlat.GetStoryDetails(storyId);
            return View(v);
        }


        //For User Edit Profile

        public IActionResult UserEditProfile()
        {
            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);

            UserDetailsViewModel u = new UserDetailsViewModel();
            u.users = _iCiPlat.GetUserDetails(userId);
            ViewBag.img = u.users.Avatar;
            ViewBag.FirstName = u.users.FirstName;
            u.allcountries = _iCiPlat.GetCountryList();
            u.allskills = _iCiPlat.GetAllSkills();
            u.userSkillsList = _iCiPlat.GetUsersSkills(userId);
            return View(u);
        }

        [HttpPost]
        public IActionResult UserEditProfile(UserDetailsViewModel u)
        {
           
           

            List<int> userSkillsIds = new List<int>();
            if (u.userSkills != null)
            {
                u.userSkills = u.userSkills.Remove(u.userSkills.Length - 1, 1);
                userSkillsIds = u.userSkills.Split(',').Select(int.Parse).ToList();
            }

            if (u.userAvatar != null)
            {
                string filename = Guid.NewGuid().ToString() + u.userAvatar.FileName;

                var filestr = new FileStream("wwwroot/images/Avatar/" + filename, FileMode.Create);
                u.userAvatar.CopyTo(filestr);
                u.users.Avatar = "/images/Avatar/" + filename;
            }

            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);

            u.users.UserId = userId;
            _iCiPlat.UpdateUserDetails(u.users, userSkillsIds);

            return RedirectToAction("userEditProfile", "Home");
        }


        public JsonResult Country()
        {
            var c = _cI_PlatformContext.Countries.ToList();
            return new JsonResult(c);
        }

        public JsonResult City(int id)
        {
            var city = _cI_PlatformContext.Cities.Where(s => s.CountryId == id).ToList();
            return new JsonResult(city);
        }


        //For Change Password In User Edit Profile Section

        public int ChangePassword(UserDetailsViewModel u)
        {
            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);
            u.users.UserId = userId;


            if (u.oldPassword == null || u.newPassword == null || u.confirmNewPassword == null)
            {
                ViewData["Error"] = "One of the field is empty";
                return -2;
            }

            if (u.newPassword != u.confirmNewPassword)
            {
                ViewData["PasswordNotMatch"] = "New Password and Confirm New Password Must Be Match";
                return -1;
            }
            else
            {
                return _iCiPlat.ChangeUserPassword(u);
            }

        }


        //For Volunteering Time Sheet

        public IActionResult VolunteeringTimeSheet()
        {
            var identity = User.Identity as ClaimsIdentity;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);

            volunteeringTimeSheetViewModel volTime = new volunteeringTimeSheetViewModel();
            volTime = _iCiPlat.GetVolunteerTimeDetails(userId);


            return View(volTime);
        }

        [HttpPost]
        public IActionResult VolunteeringTimesheet(volunteeringTimeSheetViewModel vm)
        {
            var identity = User.Identity as ClaimsIdentity;
            var suserId = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            long userId = long.Parse(suserId);


            if (vm.missionId == 0 || vm.message == null || vm.date.ToString("dd-MM-yyyy") == "01-01-0001" || (vm.hours == 0 && vm.minutes == 0 && vm.action == 0))
            {
                return RedirectToAction("VolunteeringTimesheet", "Home");
            }

            var a = _iCiPlat.AddTimeSheetEntry(userId, vm);
            return RedirectToAction("VolunteeringTimesheet", "Home");
        }

        //For Delete Record in Volunteering Time Sheet
        public bool DeleteTimesheetRecord(int timesheetId)
        {
            return _iCiPlat.DeleteTimesheetRecord(timesheetId);
        }

        //For Edit Record in Volunteering Time Sheet
        public Timesheet EditTimesheetRecord(long timesheetId)
        {
            return _iCiPlat.EditTimesheetRecord(timesheetId);
        }




        //For the Privacy Page
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LoadStory(int missionId)
        {


            var identity = User.Identity as ClaimsIdentity;
            long userId = long.Parse(identity.FindFirst(ClaimTypes.Sid).Value);
            
            ShareStoryViewModel v = _iCiPlat.GetSavedStory(userId, missionId);
            v.missionlist = _iCiPlat.DisplayMissions();
            JsonResult j = new JsonResult(v);
          
            if (v.stories != null)
            {

                var temp = new { title = v.stories.Title, Description = v.stories.Description, date = v.stories.PublishedAt.Value.ToString("yyyy-MM-dd"), url = v.url };
                JsonResult i = new JsonResult(temp);
                return i;
            }
            else
            {
                return Json(false);
            }

        }



    }
}






