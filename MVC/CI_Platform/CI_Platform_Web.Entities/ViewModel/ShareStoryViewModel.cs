using CI_Platform_Web.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class ShareStoryViewModel
    {
        public MissionListViewModel missionlist { get; set; }
        //public string  Url { get; set; }
        public Story stories { get; set; }
        public StoryMedium media { get; set; }
        public int missionId { get; set; }
        public List<IFormFile> attachment { get; set; }
    }
}
