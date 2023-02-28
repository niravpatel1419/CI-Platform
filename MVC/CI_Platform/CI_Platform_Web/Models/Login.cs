using System;
using System.Collections.Generic;

namespace CI_Platform_Web.Models
{
    public partial class Login
    {
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
    }
}
