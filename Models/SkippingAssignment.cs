using System;

namespace MMUniGraduation.Models
{
    public class SkippingAssignment
    {
        public SkippingAssignment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Title { get; set; }
        public decimal Grade { get; set; }
        public string Comment { get; set; }
        public string StudentId { get; set; }
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
