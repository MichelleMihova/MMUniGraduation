using System;

namespace MMUniGraduation.Models
{
    public class Image
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Extension { get; set; }
        public int? StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }

        //public string AddedByUserId { get; set; }

        //public virtual ApplicationUser AddedByUser { get; set; }
    }
}
