using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class StudyProgram
    {
        public StudyProgram()
        {
            //this.StudentStudyProgram = new List<StudentStudyProgram>();
            this.LectorStudyPrograms = new List<LectorStudyProgram>();
            this.Courses = new HashSet<Course>();
            this.Images = new HashSet<Image>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        //public virtual ICollection<StudentStudyProgram> StudentStudyProgram { get; set; }
        [NotMapped]
        public virtual ICollection<LectorStudyProgram> LectorStudyPrograms { get; set; }
    }
}