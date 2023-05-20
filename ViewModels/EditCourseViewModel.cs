using Microsoft.AspNetCore.Http;
using MMUniGraduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.ViewModels
{
    public class EditCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseSignature { get; set; }
        public string CourseDescription { get; set; }

        public int LectureId { get; set; }
        public string LectureName { get; set; }
        public string LectureDescription { get; set; }
        public string VideoUrl { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
        public IEnumerable<IFormFile> HWFiles { get; set; }

        public string LectureFileId { get; set; }
        public decimal MinHWGrade { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SkipCoursEndDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShow { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeToShowFile { get; set; }

        //public int StudyProgramId { get; set; }
        //public virtual StudyProgram StudyProgram { get; set; }

        public Course Course { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
