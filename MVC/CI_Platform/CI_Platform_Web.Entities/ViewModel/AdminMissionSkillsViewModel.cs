using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class AdminMissionSkillsViewModel
    {
        public List<Skill> skills { get; set; }

        public Skill skill  { get; set; }
    }
}