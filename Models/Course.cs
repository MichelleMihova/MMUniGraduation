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
            this.Lectures = new List<Lecture>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public int ParetntId { get; set; }
        public int NextCourseId { get; set; }
        //public string UserId { get; set; }
        public string CreatorId { get; set; }

        //TODO !!!
        //public string SkippingCourseSignature { get; set; }
        public bool SkipCourse { get; set; }
        //public string Exam { get; set; }
        //public decimal ExamGrade { get; set; }
        //public decimal FinalHomeworkGrade { get; set; }



        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime SkipCoursEndDate { get; set; }
      
   
        [ForeignKey("StudyProgram")]
        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
