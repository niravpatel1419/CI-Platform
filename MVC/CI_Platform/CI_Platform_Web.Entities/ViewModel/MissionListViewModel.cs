using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class MissionListViewModel
    {
        public IEnumerable<Mission> Missions { get; set; }

        public Mission mission { get; set; }

        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<Country> countries { get; set; }

        public IEnumerable<MissionTheme> MissionThemes { get; set; }
        public IEnumerable<MissionMedium> missionMedias { get; set; }

        public List<int> MissionRatingss { get; set; }
        public List<bool> FavMission { get; set; }
        public List<string> MissionApplicationlist { get; set; }

        public IEnumerable<GoalMission> Goals { get; set; }

        public List<int> seatleft { get; set; }
    }
}
