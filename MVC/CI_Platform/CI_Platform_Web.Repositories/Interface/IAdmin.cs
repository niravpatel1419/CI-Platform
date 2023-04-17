using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Repositories.Interface
{
    public interface IAdmin
    {
        public AdminViewModel GetUserDetails();

        public User FetchUserDetails(long userId);

        public bool AddUpdateUserDetails(AdminViewModel vm);
    }
}
