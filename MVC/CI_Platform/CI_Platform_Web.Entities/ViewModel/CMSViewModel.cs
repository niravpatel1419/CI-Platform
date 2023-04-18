using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class CMSViewModel
    {
        public List<CmsPage> cms { get; set; }

        public CmsPage cmsDetails { get; set; }
    }
}
