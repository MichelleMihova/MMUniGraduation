using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        //public CategoryType CategoryType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<LearningObject> VideoMaterials { get; set; }
        //public List<LearningObject> TextMaterials { get; set; }
        public string Assignments { get; set; }
        public decimal AssignmentGrade { get; set; }

        //one to many relation
        //[Key]
        //[ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public string ParetntLectureSignature { get; set; }
        public string NextLectureSignature { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDateTimeForHW { get; set; }
    }
}
