namespace MMUniGraduation.Models
{
    public class ProgramCourse
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int ProgramId { get; set; }
        public StudyProgram Program { get; set; }
    }
}
