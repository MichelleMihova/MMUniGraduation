using System;

namespace MMUniGraduation.Models
{
    public class Homework
    {
        public Homework()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string HomeworkName { get; set; }
        public string Extension { get; set; }
        public string HomeworkTitle { get; set; }
        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public string StudentId { get; set; }
        //public Student Student { get; set; }
        
        public int? LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }
    }
}
