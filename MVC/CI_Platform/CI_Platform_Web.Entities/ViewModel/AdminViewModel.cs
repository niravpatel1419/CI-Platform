using CI_Platform_Web.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class AdminViewModel
    {
        public List<User> userList { get; set; } = new List<User>();
        public User userDetails { get; set; } = new User();
        public List<Country> countryList { get; set; }
        public List<City> citylist { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
