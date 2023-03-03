
using System.ComponentModel.DataAnnotations;


namespace CI_Platform_Web.Entities.Models
{
    public class Login
    {
        [Required,EmailAddress,Display(Name ="Email Address")]
        public string? Email { get; set; }

        [Required,Display(Name ="Password")]
        public string? Password{ get; set; }
    }
}
