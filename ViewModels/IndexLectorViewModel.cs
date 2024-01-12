using AutoMapper;
using MMUniGraduation.Models;
using SmartProfil.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.ViewModels
{
    public class IndexLectorViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }

        public IEnumerable<Course> Courses { get; set; }
        public Dictionary<string, List<Course>> ProgramCourses { get; set; }
    }
}
