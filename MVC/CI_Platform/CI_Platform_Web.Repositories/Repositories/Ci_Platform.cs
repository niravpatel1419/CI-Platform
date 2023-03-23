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
    public class Ci_Platform : ICi_Platform
    {
        private readonly CI_PlatformContext _cI_PlatformContext;

        public Ci_Platform(CI_PlatformContext cI_PlatformContext)
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

        public MissionListViewModel DisplayMissions()
        {
            MissionListViewModel missions = new MissionListViewModel();
            missions.Missions = _cI_PlatformContext.Missions.ToList();
            missions.Cities = _cI_PlatformContext.Cities.ToList();
            missions.countries = _cI_PlatformContext.Countries.ToList();
            missions.MissionThemes = _cI_PlatformContext.MissionThemes.ToList();

            return missions;
        }

    }
}
