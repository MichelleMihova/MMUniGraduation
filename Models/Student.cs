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
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //public string Town { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public bool ShowTextMaterials { get; set; }
        public bool ShowVideoMaterials { get; set; }

        //remove Photo
        //public string Photo { get; set; }
        //public virtual ICollection<Image> Images { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? EndDateTime { get; set; }

        //[NotMapped]
        //public ICollection<Course> PassedCourses { get; set; }
        [NotMapped]
        public ICollection<Course> CurrentCourses { get; set; }

        // Maybe we need only this and above ones to be removed
        public ICollection<StudentCourses> Passed { get; set; }
        [NotMapped]
        public IDictionary<Course, decimal> PassedCoursesGrade { get; set; }
    }
}
