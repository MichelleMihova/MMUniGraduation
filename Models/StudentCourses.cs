using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Models
{
    public class StudentCourses
    {
        public int StudentId { get; set; }
        public Student User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public bool IsPassed { get; set; }
        public decimal FinalGrade { get; set; }
    }
}
