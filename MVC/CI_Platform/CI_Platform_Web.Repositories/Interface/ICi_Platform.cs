using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Repositories.Interface
{
    public interface ICi_Platform
    {
        public User Login(User userLogin);
        public bool IsRegister(User userRegister);
        public bool IsEmailExist(string email);
        public void ForgetPassword(string email, string token);
        public MissionListViewModel DisplayMissions();

    }
}
