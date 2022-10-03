namespace MMUniGraduation.Models
{
    public class LectorStudyProgram
    {
        public int LectorId { get; set; }
        public Lector Lector { get; set; }
        public int StudyProgramId { get; set; }
        public StudyProgram StudyProgram { get; set; }
    }
}
