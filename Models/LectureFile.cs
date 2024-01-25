using System;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.Models
{
    public class LectureFile
    {
        public LectureFile()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
        public decimal MinHWGrade { get; set; }
        public decimal MaxHWGrade { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }

        public int? LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
