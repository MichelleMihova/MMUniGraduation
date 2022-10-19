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
            //this.StudentCourses = new List<StudentCourse>();
            this.PassedCourses = new List<Course>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Town { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }

        //[ForeignKey("Course")]
        //public int CurrenCourseId { get; set; }
        public Course CurrentCourse { get; set; }

        //Many to many relation
        public ICollection<StudentStudyProgram> CompleatedStudyPrograms { get; set; }
        //public ICollection<StudentCourse> StudentCourses { get; set; }

        //new - all passed student courses
        [NotMapped]
        public ICollection<Course> PassedCourses { get; set; }

    }
}
