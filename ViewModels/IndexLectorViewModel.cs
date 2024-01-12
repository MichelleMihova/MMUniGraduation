using MMUniGraduation.Models;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class IndexLectorViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }

        public IEnumerable<Course> Courses { get; set; }
        public Dictionary<string, List<Course>> ProgramCourses { get; set; }
    }
}
