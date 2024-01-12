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
            this.Homeworks = new HashSet<Homework>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int ParetntLectureId { get; set; }
        public int NextLectureId { get; set; }
        public string VideoUrl { get; set; }
        public string CreatorId { get; set; }
        //Exam lecture
        public bool IsExam { get; set;  }
        //Exemption lecture
        public bool IsExemption { get; set;  }
        //Retake exam lecture
        public bool IsRetakeExam { get; set;  }
        //may remove RequiredGrade or to use it for showing lecture
        public decimal RequiredGrade { get; set;  }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTimeForHW { get; set; }
       
        public ICollection<LectureFile> TextMaterials { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
    }
}
