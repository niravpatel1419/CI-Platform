using CI_Platform_Web.Entities.Models;
using Microsoft.AspNetCore.Http;
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
        public IFormFile userAvatar { get; set; }

        [Required(ErrorMessage = "Please enter the name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the surname")]
        public string? LastName { get; set; }

        public string? EmployeeId { get; set; }

        public string? Manager { get; set; }

        public string? Title { get; set; }

        public string? Department { get; set; }

        [Required(ErrorMessage = "Please enter the profile details")]
        public string? ProfileText { get; set; }

        [Required(ErrorMessage = "Please fill the detail")]
        public string? WhyIVolunteer { get; set; }

        [Required(ErrorMessage = "Please select your country")]
        public long? CountryId { get; set; }

        [Required(ErrorMessage = "Please select your city")]
        public long? CityId { get; set; }

        public string? Availability { get; set; }

        public string? LinkedInUrl { get; set; }


    }
}
