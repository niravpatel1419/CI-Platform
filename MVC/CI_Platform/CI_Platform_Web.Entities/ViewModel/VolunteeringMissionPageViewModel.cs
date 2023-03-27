using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class VolunteeringMissionPageViewModel
    {
        public Mission missions { get; set; }
        public City cities { get; set; }
        public Country countries { get; set; }
        public MissionTheme themes { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Comment> comments { get; set; }
        public MissionListViewModel relatedMissions { get; set; }
        public string goaltimestring { get; set; }
        public string Applied { get; set; }
    }
}
