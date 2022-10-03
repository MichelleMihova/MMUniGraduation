using System.Collections.Generic;

namespace MMUniGraduation.Models
{
    public class StudyProgram
    {
        public StudyProgram()
        {
            this.StudentStudyProgram = new List<StudentStudyProgram>();
            this.LectorStudyPrograms = new List<LectorStudyProgram>();
            this.Courses = new HashSet<Course>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<StudentStudyProgram> StudentStudyProgram { get; set; }
        public virtual ICollection<LectorStudyProgram> LectorStudyPrograms { get; set; }
    }
}