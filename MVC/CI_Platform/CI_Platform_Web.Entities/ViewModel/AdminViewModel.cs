using CI_Platform_Web.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Entities.ViewModel
{
    public class AdminViewModel
    {
        public List<User> userList { get; set; } = new List<User>();

        public List<Country> countryList { get; set; }

        public List<City> citylist { get; set; }

        public IFormFile Avatar { get; set; }

         public long UserId { get; set; }
       
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter first name.")]
        public string? FirstName { get; set; }

     
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter surname.")]
        public string? LastName { get; set; }

        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter an email.")]
        public string Email { get; set; } = null!;

       
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter the password.")]
        public string Password { get; set; } = null!;

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select the country.")]
        public long? CountryId { get; set; }

        
        [ System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select the city.")]
        public long? CityId { get; set; }

        public int Status { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter profile details")]
        public string? ProfileText { get; set; }

    }
}
