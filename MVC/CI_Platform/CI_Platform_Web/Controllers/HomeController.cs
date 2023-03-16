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

namespace CI_Platform_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CI_PlatformContext _cI_PlatformContext;
        int i = 0, i1 = 0, j = 0, j1 = 0, k = 0, k1 = 0;

        public HomeController(ILogger<HomeController> logger, CI_PlatformContext cI_PlatformContext)
        {
            _logger = logger;
            _cI_PlatformContext = cI_PlatformContext;
        }


        public IActionResult Index()
        {
            return View();
        }


        //For the User Registration

        [HttpGet]
        public IActionResult registration()
        {
            User user = new User();
            return View(user);
        }


        [HttpPost]
        public IActionResult registration(User user)
        {
            var compare = _cI_PlatformContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (compare != null)
            {

                ViewBag.RegEmail = "Email Address Already Exists.Please use a different email address";

            }
            else
            {
                _cI_PlatformContext.Users.Add(user);
                _cI_PlatformContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();

        }



        //For the ForgotPassword

        [HttpGet]
        public IActionResult forgotPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult forgotPassword(ForgotPassword _forogtpass)
        {

            var status = _cI_PlatformContext.Users.FirstOrDefault(m => m.Email == _forogtpass.Email);
            if (status == null)
            {

                TempData["Error"] = "Email id is invalid or You are not registerd";
                return View();

            }

            // Generate a password reset token for the user
            var token = Guid.NewGuid().ToString();


            // Store the token in the password resets table with the user's email
            var passwordReset = new CI_Platform_Web.Entities.Models.PasswordReset
            {
                Email = _forogtpass.Email,
                Token = token,
            };

            _cI_PlatformContext.Add(passwordReset);
            _cI_PlatformContext.SaveChanges();


            // Send an email with the password reset link to the user's email address
            var resetLink = Url.Action("resetPassword", "Home", new { email = _forogtpass.Email, token }, Request.Scheme);

            var fromAddress = new MailAddress("computerengineermeet@gmail.com", "CI_Platform");
            var toAddress = new MailAddress(_forogtpass.Email);
            var subject = "Password reset request";
            var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
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

            return RedirectToAction("ForgotPasswordConfirmation", "Home");

        }



        //For ForgotPasswordConfirmation

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }



        // For ResetPassword

        [HttpGet]
        public IActionResult resetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password Reset Token");
            }
            return View();
        }

        [HttpPost]
        public IActionResult resetPassword(ResetPasswordView model)

        {
            if (ModelState.IsValid)
            {
                var user = _cI_PlatformContext.Users.FirstOrDefault(m => m.Email == model.Email);
                if (user == null)
                {
                    return RedirectToAction("forgotPassword", "Home");

                }

                user.Password = model.Password;
                _cI_PlatformContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("forgotPassword", "Home");
        }




        //For Logout

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }




        //For LandingPage

        public IActionResult home(string searchQuery, long id, int? pageIndex, int Order, long[] ACountries, long[] ACities, long[] ATheme)
        {


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

            List<GoalMission> goalMission = _cI_PlatformContext.GoalMissions.ToList();
            ViewBag.goalMission = goalMission;


            //For Shown the Mission Details On the Card

            List<Mission> mission = _cI_PlatformContext.Missions.ToList();
            List<Mission> finalmission = _cI_PlatformContext.Missions.ToList();
            List<Mission> newmission = _cI_PlatformContext.Missions.ToList();

            foreach (var item in mission)
            {
                var City = _cI_PlatformContext.Cities.FirstOrDefault(u => u.CityId == item.CityId);
                var Theme = _cI_PlatformContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == item.ThemeId);

            }

            mission = _cI_PlatformContext.Missions.ToList();


            //For Country Filter
            if (ACountries != null && ACountries.Length > 0)
            {

                foreach (var c1 in ACountries)
                {
                    //mission = mission.Where(m => m.CountryId == country).ToList();
                    if (i == 0)
                    {
                        mission = mission.Where(m => m.CountryId == c1 + 500).ToList();
                        i++;
                    }

                    finalmission = newmission.Where(m => m.CountryId == c1).ToList();

                    mission.AddRange(finalmission);
                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("noMissionFound", "Home");
                    }
                    ViewBag.countryId = c1;
                    if (ViewBag.countryId != null)
                    {
                        var countryElement = _cI_PlatformContext.Countries.Where(m => m.CountryId == c1).ToList();
                        if (i1 == 0)
                        {
                            countryElements = _cI_PlatformContext.Countries.Where(m => m.CountryId == c1 + 50000).ToList();
                            i1++;
                        }
                        countryElements.AddRange(countryElement);
                        //var c1 = _CiPlatformContext.Countries.FirstOrDefault(m => m.CountryId == country);
                        //ViewBag.country = c1.Name;
                    }
                }
                ViewBag.countries = countryElements;
                //Countries = _CiPlatformContext.Countries.ToList();


            }

            //For City filter
            if (ACities != null && ACities.Length > 0)
            {
                foreach (var city in ACities)
                {
                    //mission = mission.Where(m => m.CityId == city).ToList();

                    if (j == 0)
                    {
                        mission = mission.Where(m => m.CityId == city + 500).ToList();
                        j++;
                    }

                    finalmission = newmission.Where(m => m.CityId == city).ToList();

                    mission.AddRange(finalmission);
                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("noMissionFound", "Home");
                    }
                    ViewBag.cities = city;
                    if (ViewBag.city != null)
                    {
                        var city1 = _cI_PlatformContext.Cities.Where(m => m.CityId == city).ToList();
                        if (j1 == 0)
                        {
                            Cities = _cI_PlatformContext.Cities.Where(m => m.CityId == city + 50000).ToList();
                            j1++;
                        }
                        Cities.AddRange(city1);
                        //var c1 = _CiPlatformContext.Cities.FirstOrDefault(m => m.CityId == city);
                        //ViewBag.city = c1.Name;
                    }
                }
                ViewBag.cities = Cities;
                Cities = _cI_PlatformContext.Cities.ToList();


            }

            //For theme filter
            if (ATheme != null && ATheme.Length > 0)
            {
                foreach (var theme in ATheme)
                {

                    if (k == 0)
                    {
                        mission = mission.Where(m => m.ThemeId == theme + 500).ToList();
                        k++;
                    }

                    finalmission = newmission.Where(m => m.ThemeId == theme).ToList();

                    mission.AddRange(finalmission);

                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("noMissionFound", "Home");
                    }
                    ViewBag.theme = theme;
                    if (ViewBag.theme != null)
                    {
                        var theme1 = _cI_PlatformContext.MissionThemes.Where(m => m.MissionThemeId == theme).ToList();
                        if (k1 == 0)
                        {
                            Themes = _cI_PlatformContext.MissionThemes.Where(m => m.MissionThemeId == theme + 50000).ToList();
                            k1++;
                        }
                        Themes.AddRange(theme1);
                        //var c1 = _CiPlatformContext.MissionThemes.FirstOrDefault(m => m.MissionThemeId == theme);
                        //ViewBag.theme = c1.Title;
                    }
                }
                ViewBag.theme = Themes;
                Themes = _cI_PlatformContext.MissionThemes.ToList();

            }



            //For Sort By Feature

            switch (Order)
            {
                case 1:
                    mission = newmission.OrderByDescending(m => m.StartDate).ToList();
                    ViewBag.sortby = "Newest";
                    break;
                case 2:
                    mission = newmission.OrderBy(m => m.StartDate).ToList();
                    ViewBag.sortby = "Oldest";
                    break;
                case 3:
                    mission = mission.OrderBy(m => int.Parse(m.Availability)).ToList();
                    break;
                case 4:
                    mission = mission.OrderBy(m => m.EndDate).ToList();
                    break;
            }
            //    case "Highest seats":
            //        mission = mission.OrderByDescending(m => int.Parse(m.Availability)).ToList();
            //        break;
            //    case "Registration deadline":
            //        mission = mission.OrderBy(m => m.EndDate).ToList();
            //        break;

            //}


            // Get the current URL
            UriBuilder uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host);
            if (Request.Host.Port.HasValue)
            {
                uriBuilder.Port = Request.Host.Port.Value;
            }
            uriBuilder.Path = Request.Path;

            // Remove the query parameter you want to exclude
            var query = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            query.Remove("pageIndex");
            uriBuilder.Query = query.ToString();



            ViewBag.currentUrl = uriBuilder.ToString();


            //for search the mission

            if (searchQuery != null)
            {
                mission = _cI_PlatformContext.Missions.Where(m => m.Title.Contains(searchQuery)).ToList();
                ViewBag.searchQuery = searchQuery;
                if (mission.Count() == 0)
                {
                    return RedirectToAction("noMissionFound", "Home");
                }
            }

            //For the Pagination

            int pageSize = 6;
            int skip = (pageIndex ?? 0) * pageSize;
            var Missions = mission.Skip(skip).Take(pageSize).ToList();


            int totalMissions = mission.Count();
            ViewBag.TotalMission = totalMissions;

            ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            ViewBag.CurrentPage = pageIndex ?? 0;

            return View(Missions);

            //return View(Missions);

        }


        /* public IActionResult home(string searchQuery, string sortOrder, int? pageIndex)
        {

            //For shown the Country list in the Dropdown
                
            List<Country> country = _cI_PlatformContext.Countries.ToList();
            ViewBag.Country = country;

            //For shown the City list in the Dropdown

            List<City> city = _cI_PlatformContext.Cities.ToList();
            ViewBag.City = city;

            //For shown the Theme list in  the Dropdown

            List<MissionTheme> missionThemes = _cI_PlatformContext.MissionThemes.ToList();
            ViewBag.MissionThemes = missionThemes;

            //For shown the Skills list in the Dropdown

            List<Skill> skills = _cI_PlatformContext.Skills.ToList();
            ViewBag.Skill = skills;


            //For Shown the Mission Details On the Card

            List<Mission> mission = _cI_PlatformContext.Missions.ToList();
            foreach (var i in mission)
            {
                var City = _cI_PlatformContext.Cities.FirstOrDefault(u => u.CityId == i.CityId);
                var Theme = _cI_PlatformContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == i.ThemeId);
                var Country = _cI_PlatformContext.Countries.FirstOrDefault(u => u.CountryId == i.CountryId);
            }



            //for sort by filter

            switch (sortOrder)
            {
                case "Newest":
                    mission = (List<Mission>)mission.OrderByDescending(m => m.StartDate).ToList();
                        break;

                case "Oldest":
                    mission = (List<Mission>)mission.OrderBy(m => m.StartDate).ToList();
                    break;

                case "Lowest available seats":
                    mission = (List<Mission>)mission.OrderBy(m => m.Availability).ToList();
                    break;

                case "Highest available seats":
                    mission = (List<Mission>)mission.OrderByDescending(m => m.Availability).ToList();
                    break;

                case "My favourites":
                    mission = (List<Mission>)mission.OrderBy(m => m.FavouriteMissions).ToList();
                    break;

                case "Registration deadline":
                    mission = (List<Mission>)mission.OrderBy(m => m.EndDate).ToList();
                    break;

                default:
                    mission = (List<Mission>)mission.OrderByDescending(m => m.StartDate).ToList();
                    break;
            }



            //for search the mission

            if (searchQuery != null)
            {
                mission = _cI_PlatformContext.Missions.Where(m => m.Title.Contains(searchQuery)).ToList();
                ViewBag.searchQuery = searchQuery;
                if (mission.Count() == 0)
                {
                    return RedirectToAction("noMissionFound");
                }
            }


            //For the Pagination

            int pageSize = 6;
            int skip = (pageIndex ?? 0) * pageSize;
            var Missions = mission.Skip(skip).Take(pageSize).ToList();

    
            int totalMissions = mission.Count();
            ViewBag.TotalMission = totalMissions;

            ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            ViewBag.CurrentPage = pageIndex ?? 0;

            return View(Missions);

        }*/



        //For the no mission found

        public IActionResult noMissionFound()
        {
            return View();
        }






        //For the volunteering mission 

        public IActionResult volunteeringMission(int missID)
        {

            //For shown the Theme list in  the Dropdown

            List<MissionTheme> missionThemes = _cI_PlatformContext.MissionThemes.ToList();
            ViewBag.MissionThemes = missionThemes;


            //For mission info

            var mission = _cI_PlatformContext.Missions.FirstOrDefault(i => i.MissionId == missID);
            ViewBag.mission = mission;

            if (mission.MissionType == "goal")
            {
                ViewBag.goaltext = _cI_PlatformContext.GoalMissions.Where(s => s.MissionId == missID).ToList()[0].GoalObjectiveText.ToString();
            }
            else
            {
                ViewBag.goaltext = "It is a time base mission";
            }

            City city = _cI_PlatformContext.Cities.FirstOrDefault(s => s.CityId == mission.CityId);
            ViewBag.citylist = city;

            MissionTheme theme = _cI_PlatformContext.MissionThemes.FirstOrDefault(s => s.MissionThemeId == mission.ThemeId);
            ViewBag.themelist = theme;

            var orgName = _cI_PlatformContext.Missions.FirstOrDefault(i => i.OrganizationName == mission.OrganizationName);
            ViewBag.orgname = orgName;


            //For the releated mission

            long themeid = _cI_PlatformContext.Missions.FirstOrDefault(x => x.MissionId == missID).ThemeId;
            long cityid = _cI_PlatformContext.Missions.FirstOrDefault(x => x.MissionId == missID).CityId;
            //long countryid = _cI_PlatformContext.Missions.FirstOrDefault(x => x.MissionId == missID).CountryId;

            IEnumerable<Mission> relatedmission1 = _cI_PlatformContext.Missions.Where(x => (x.CityId == cityid || x.ThemeId == themeid) && x.MissionId != missID).ToList().Take(3);
            ViewBag.relatedmission1 = relatedmission1;

            if (relatedmission1.Count() == 0)
            {
                ViewData["NoRelatedMission"] = "No Related Mission Available";
            }


            //For Add to Favourite Mission
            ViewBag.mid = missID;
            List<FavouriteMission> f = _cI_PlatformContext.FavouriteMissions.Where(x => x.UserId == (long)HttpContext.Session.GetInt32("Id") && x.MissionId == missID).ToList();
            if (f.Count == 0)
            {
                ViewBag.favmission = "Add";
            }

            //For Mission Rating
            List<MissionRating> rr = _cI_PlatformContext.MissionRatings.Where(x => x.UserId == (long)HttpContext.Session.GetInt32("Id") && x.MissionId == missID).ToList();
            if (rr.Count != 0)
            {
                ViewBag.rate = rr[0].Rating;
            }

            return View();


        }



        //For Add to Favourite Mission

        public int addToFav(int id, int mid)
        {

            List<FavouriteMission> r = _cI_PlatformContext.FavouriteMissions.Where(x => x.UserId == id && x.MissionId == mid).ToList();

            if (r.Count == 0)
            {
                FavouriteMission f = new FavouriteMission();
                f.MissionId = mid;
                f.UserId = id;
                _cI_PlatformContext.FavouriteMissions.Add(f);
                _cI_PlatformContext.SaveChanges();

                return 1;
            }

            else
            {
                FavouriteMission fav = _cI_PlatformContext.FavouriteMissions.Where(x => x.UserId == id && x.MissionId == mid).FirstOrDefault();
                _cI_PlatformContext.Remove(fav);
                _cI_PlatformContext.SaveChanges();
                return 0;
            }

        }

        //For User Rating in Volunteering Mission Page

        public int Ratee(int uid, int mid, int rating)
        {
            List<MissionRating> p = _cI_PlatformContext.MissionRatings.Where(x => x.UserId == uid && x.MissionId == mid).ToList();
            if (p.Count == 0)
            {
                MissionRating r = new MissionRating();
                r.Rating = rating;
                r.MissionId = mid;
                r.UserId = uid;
                _cI_PlatformContext.MissionRatings.Add(r);
                _cI_PlatformContext.SaveChanges();

                return 1;
            }
            else
            {
                /*  MissionRating r = new MissionRating();
                  r.MissionRatingId = p[0].MissionRatingId;*/
                p[0].Rating = rating;
                /* r.MissionId = mid;
                 r.UserId = uid;*/
                _cI_PlatformContext.MissionRatings.Update(p[0]);
                _cI_PlatformContext.SaveChanges();

                return 3;
            }

        }



        public IActionResult storyListingpage()
        {
            return View();
        }


        //For the Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public JsonResult Themes()
        {
            var theme = _cI_PlatformContext.MissionThemes.ToList();
            return new JsonResult(theme);
        }


    }
}






