using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.Models
{
    public class Lecture
    {
        public Lecture()
        {
            this.TextMaterials = new HashSet<LectureFile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Assignments { get; set; }
        public decimal AssignmentGrade { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int ParetntLectureId { get; set; }
        public int NextLectureId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTimeForHW { get; set; }
        public ICollection<LectureFile> TextMaterials { get; set; }
        public IList<LearningObject> VideoMaterials { get; set; }


        //public CategoryType CategoryType { get; set; }
        //one to many relation
        //[Key]
        //[ForeignKey("Course")]
    }
}
