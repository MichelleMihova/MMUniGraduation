using System;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.Models
{
    public class StudentCourses
    {
        public int StudentId { get; set; }
        public Student User { get; set; }
        public int CourseId { get; set; }
        public int ProgramId { get; set; }
        public Course Course { get; set; }
        public bool IsPassed { get; set; }
        public decimal FinalGrade { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateTimeForSkipping { get; set; }
    }
}
