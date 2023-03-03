using System.ComponentModel.DataAnnotations;

namespace CI_Platform_Web.Entities.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
