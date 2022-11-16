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

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }

        public int? LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

        //public string AddedByUserId { get; set; }

        //public virtual ApplicationUser AddedByUser { get; set; }
    }
}
