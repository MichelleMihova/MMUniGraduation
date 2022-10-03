using System.Collections.Generic;

namespace MMUniGraduation.Models
{
    public class StudentStudyProgram
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int StudyProgramId { get; set; }
        public StudyProgram StudyProgram { get; set; }
    }
}
