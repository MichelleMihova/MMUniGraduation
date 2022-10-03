namespace MMUniGraduation.Models
{
    public class CourseLecture
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
    }
}
