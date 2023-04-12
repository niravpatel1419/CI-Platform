using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class volunteeringTimeSheetViewModel
    {
        public IEnumerable<MissionApplication> timeMissions { get; set; }
        public IEnumerable<MissionApplication> goalMissions { get; set; }
        public List<Mission> allMissions { get; set; }
        public int timesheetPrimary { get; set; } = 0;
        public int missionId { get; set; }
        public string missionType { get; set; }
        public string message { get; set; }
        public int action { get; set; }
        public int minutes { get; set; }
        public int hours { get; set; }
        public List<Timesheet> goalTimesheetList { get; set; }
        public List<Timesheet> timeTimesheetList { get; set; }
        public DateTime date { get; set; }
    }
}
