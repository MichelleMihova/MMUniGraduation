using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Course
    {
        public Course()
        {
            //this.StudentCourses = new List<StudentCourse>();
            this.Lectures = new List<Lecture>();
        }

        public int Id { get; set; }
        //public CategoryType CategoryType { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public string ParetntID { get; set; }
        public string NextCourseSignature { get; set; }
        public string SkippingCourseSignature { get; set; }
        public bool SkipCourse { get; set; }
        [DataType(DataType.DateTime)]
        //The DisplayFormat attribute is used to explicitly specify the date format
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SkipCoursEndDate { get; set; }

        //public int CourseId { get; set; }
        //public ICollection<Program> Program { get; set; }
        //[ForeignKey("StudyProgram")]
        //new
        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
        //public SkippingAssignment SkippingAssignment { get; set; }
        public string Exam { get; set; }
        public decimal ExamGrade { get; set; }
        public decimal FinalHomeworkGrade { get; set; }
        //one to many relation
        public virtual ICollection<Lecture> Lectures { get; set; }

        //Many to many relation
        //public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
