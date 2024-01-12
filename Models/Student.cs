using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Student
    {
        public Student()
        {
            this.CurrentCourses = new List<Course>();
            this.Passed = new List<StudentCourses>();
            this.Photos = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool ShowTextMaterials { get; set; }
        public bool ShowVideoMaterials { get; set; }
        public virtual ICollection<Image> Photos { get; set; }

        [NotMapped]
        public ICollection<Course> CurrentCourses { get; set; }

        // Maybe we need only this and above ones to be removed
        public ICollection<StudentCourses> Passed { get; set; }
        [NotMapped]
        public IDictionary<Course, decimal> PassedCoursesGrade { get; set; }
    }
}
