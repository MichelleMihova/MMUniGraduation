using System;

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

        public int? LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }

        //public string AddedByUserId { get; set; }

        //public virtual ApplicationUser AddedByUser { get; set; }
    }
}
