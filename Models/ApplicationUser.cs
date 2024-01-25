﻿using Microsoft.AspNetCore.Identity;

namespace MMUniGraduation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string TeacherToken { get; set; }
    }
}
