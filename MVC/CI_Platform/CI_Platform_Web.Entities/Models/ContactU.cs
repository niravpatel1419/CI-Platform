using System;
using System.Collections.Generic;

namespace CI_Platform_Web.Entities.Models
{
    public partial class ContactU
    {
        public long ContactusId { get; set; }
        public long UserId { get; set; }
        public string? ContactSubject { get; set; }
        public string? ContactMessage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
