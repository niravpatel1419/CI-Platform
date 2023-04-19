using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class AdminMissionViewModel
    {
        public IEnumerable<Mission> Missions { get; set; }

        public Mission mission { get; set; }

        public List<Country> countries { get; set; }

        public List<Skill> Skill { get; set; }

        public List<int> missionSkills { get; set; }

        public List<MissionTheme> missionTheme { get; set; }

        public MissionDocument missionDocument { get; set; }
        
    }
}
