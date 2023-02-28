﻿using System;
using System.Collections.Generic;

namespace CI_Platform_Web.Models
{
    public partial class MissionTheme
    {
        public MissionTheme()
        {
            Missions = new HashSet<Mission>();
        }

        public long MissionThemeId { get; set; }
        public string? Title { get; set; }
        public byte Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }
    }
}
