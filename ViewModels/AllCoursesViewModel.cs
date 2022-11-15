using MMUniGraduation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.ViewModels
{
    public class AllCoursesViewModel
    {
        //public AllCoursesViewModel()
        //{
        //    this.AllCourses = HashSet<Course>();
        //}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public string NextCourseName { get; set; }
        public IEnumerable<Course> AllCourses { get; set; }
    }
}
