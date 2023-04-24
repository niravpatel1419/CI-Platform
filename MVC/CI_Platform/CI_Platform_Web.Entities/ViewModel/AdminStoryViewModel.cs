using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class AdminStoryViewModel
    {
        public List<Story> storyList { get; set; } = new List<Story>();

        public List<User> userList { get; set; } = new List<User>();

        public List<Mission> missionList { get; set; } = new List<Mission>();

        public Story storyDetails { get; set; }

        public User userDetails { get; set; }

        public string userName { get; set; }

        public Mission missionDetails { get; set; }

        public List<string> storyimages { get; set; } = new List<string>();
    }
}
