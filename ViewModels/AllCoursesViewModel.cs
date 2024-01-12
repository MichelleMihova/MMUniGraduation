using MMUniGraduation.Models;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class AllCoursesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public string NextCourseName { get; set; }
        public Course CurrentUserCourse { get; set; }
        public IEnumerable<Course> AllCourses { get; set; }
        public IEnumerable<Course> PassedCourses { get; set; }
        public IEnumerable<Course> CurrentCourses { get; set; }
    }
}
