using MMUniGraduation.Models;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class AssessmentsViewModel
    {
        public int ProgramId { get; set; }
        public int CourseId { get; set; }
        public int LectureId { get; set; }
        public string HomeworkId { get; set; }
        public decimal HomeworkGrade { get; set; }
        public string HomeworkComment { get; set; }
        public string KindOfAssessment { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Course> AllCourses { get; set; }
        public IEnumerable<KeyValuePair<string, string>> StudyPrograms { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Courses { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Lectures { get; set; }
    }
}
