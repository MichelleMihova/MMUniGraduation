using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class Course
    {
        public Course()
        {
            this.Lectures = new List<Lecture>();
            this.SkippingCourseMaterials = new HashSet<LectureFile>();
            this.SkippingAssignments = new HashSet<SkippingAssignment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public int ParetntId { get; set; }
        public int NextCourseId { get; set; }
        public string CreatorId { get; set; }
        public ICollection<LectureFile> SkippingCourseMaterials { get; set; }
        public decimal RequiredSkippingCourseGrade { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CourseStartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime SkipCoursEndDate { get; set; }
      
   
        [ForeignKey("StudyProgram")]
        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public ICollection<SkippingAssignment> SkippingAssignments { get; set; }
    }
}
