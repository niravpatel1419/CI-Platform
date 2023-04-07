using CI_Platform_Web.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class UserDetailsViewModel
    {
        public User users { get; set; } = new User();

        [Required]
        public string oldPassword { get; set; }

        [Required]
        public string newPassword { get; set; }

        [Required]
        [Compare ("newPassword",ErrorMessage ="Confirm Password And New Password Must Watch")]
        public string confirmNewPassword { get; set; }

        public string cityName { get; set; }

        public List<Country> allcountries { get; set; }

        public List<Skill> allskills { get; set; }

        public string userSkills { get; set; }

        public List<UserSkill> userSkillsList { get; set; }
    }
}
