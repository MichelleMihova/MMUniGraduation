using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models.Create
{
    public class CreateCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public int StudyProgramId { get; set; }
        public int ParetntId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime SkipCoursEndDate { get; set; }
       
        [NotMapped]
        public IEnumerable<KeyValuePair<string, string>> StudyPrograms { get; set; }
        [NotMapped]
        public IEnumerable<KeyValuePair<string, string>> Courses { get; set; }
    }
}
