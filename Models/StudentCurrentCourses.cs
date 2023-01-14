using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Models
{
    public class StudentCurrentCourses
    {
        public int StudentId { get; set; }
        public Student User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
