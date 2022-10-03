using System.Collections.Generic;

namespace MMUniGraduation.Models
{
    public class Lector
    {
        public Lector()
        {
            this.LectorStudyPrograms = new List<LectorStudyProgram>();
        }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        //public List<Course> Courses { get; set; }
        //public List<Lecture> Assignments { get; set; }
        public ICollection<LectorStudyProgram> LectorStudyPrograms { get; set; }
    }
}
