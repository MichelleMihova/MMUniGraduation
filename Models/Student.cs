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
            this.CompleatedStudyPrograms = new List<StudentStudyProgram>();
            this.PassedCourses = new List<Course>();
            this.Passed = new List<StudentCourses>();
            //this.Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Town { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool ShowTextMaterials { get; set; }
        public bool ShowVideoMaterials { get; set; }

        //remove Photo
        public string Photo { get; set; }
        //public virtual ICollection<Image> Images { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateTime { get; set; }

        //[ForeignKey("Course")]
        public int? CurrentCourseId { get; set; }
        public virtual Course CurrentCourse { get; set; }

        [NotMapped]
        public ICollection<StudentStudyProgram> CompleatedStudyPrograms { get; set; }
        [NotMapped]
        public ICollection<Course> PassedCourses { get; set; }
        public ICollection<Course> CurrentCourses { get; set; }

        // Maybe we don't need it:
        public ICollection<StudentCourses> Passed { get; set; }

    }
}
