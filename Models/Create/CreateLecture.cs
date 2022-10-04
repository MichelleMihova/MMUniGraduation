using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Models.Create
{
    public class CreateLecture
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }

        public int ParetntLectureId { get; set; }
        //public string NextLectureSignature { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTimeForHW { get; set; }

        [NotMapped]
        public IEnumerable<KeyValuePair<string, string>> Courses { get; set; }
        [NotMapped]
        public IEnumerable<KeyValuePair<string, string>> AllLectures { get; set; }
    }
}
