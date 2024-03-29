﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Lector
    {
        public Lector()
        {
            Courses = new HashSet<Course>();
            this.Photos = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Image> Photos { get; set; }

        [NotMapped]
        public IEnumerable<Course> Courses { get; set; }

        [NotMapped]
        public Dictionary<string, List<Course>> ProgramCourses { get; set; }
    }
}
