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

        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<Country> countries { get; set; }

        public IEnumerable<MissionTheme> MissionThemes { get; set; }

        public List<int> MissionRatingss { get; set; }
    }
}
