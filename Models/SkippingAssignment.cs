using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class SkippingAssignment
    {
        public int Id { get; set; }
        //public CategoryType CategoryType { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string ParetntID { get; set; }
        public string NexrCourseSignature { get; set; }
        public string NextSkippingSignature { get; set; }
        public string Assignment { get; set; }
        public decimal Grade { get; set; }
        public bool IsPassed { get; set; }

        //We need student's last learning object to know where we to return him if he didn't pass the skipping assignment
        //[ForeignKey("Course")]
        public int CourseId { get; set; }
        //public Course Course { get; set; }
       // public ICollection<Student> Students { get; set; }
    }
}
