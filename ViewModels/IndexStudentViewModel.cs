using MMUniGraduation.Models;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class IndexStudentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Image { get; set; }

        public ICollection<Course> CurrentCourses { get; set; }
        public IDictionary<Course, decimal> PassedCoursesGrade { get; set; }
    }
}
