using System.ComponentModel.DataAnnotations;

namespace CI_Platform_Web.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
