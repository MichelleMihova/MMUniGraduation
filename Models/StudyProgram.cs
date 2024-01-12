using System.Collections.Generic;

namespace MMUniGraduation.Models
{
    public class StudyProgram
    {
        public StudyProgram()
        {
            this.Courses = new HashSet<Course>();
            this.Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}