using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Lector
    {
        public Lector()
        {
            this.LectorStudyPrograms = new List<LectorStudyProgram>();
            Courses = new HashSet<Course>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        //public List<Course> Courses { get; set; }
        //public List<Lecture> Assignments { get; set; }
        [NotMapped]
        public ICollection<LectorStudyProgram> LectorStudyPrograms { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
