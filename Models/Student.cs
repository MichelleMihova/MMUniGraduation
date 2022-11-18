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
        public string Photo { get; set; }

        //[ForeignKey("Course")]
        public int? CurrentCourseId { get; set; }
        public virtual Course CurrentCourse { get; set; }

        [NotMapped]
        public ICollection<StudentStudyProgram> CompleatedStudyPrograms { get; set; }
        [NotMapped]
        public ICollection<Course> PassedCourses { get; set; }
        
        // Maybe we don't need it:
        public ICollection<StudentCourses> Passed { get; set; }

    }
}
