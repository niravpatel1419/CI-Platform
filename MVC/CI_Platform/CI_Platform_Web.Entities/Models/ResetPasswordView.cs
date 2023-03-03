using System.ComponentModel.DataAnnotations;

namespace CI_Platform_Web.Entities.Models
{
    public class ResetPasswordView
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }   

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }    

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string? ConfirmPassword { get; set; }

        public string? Token { get; set; }
    }
}
