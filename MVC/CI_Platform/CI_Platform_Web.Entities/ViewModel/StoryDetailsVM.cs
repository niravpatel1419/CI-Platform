using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class StoryDetailsVM
    {
        public Story stories { get; set; }
        public List<User> users { get; set; }
    }
}
