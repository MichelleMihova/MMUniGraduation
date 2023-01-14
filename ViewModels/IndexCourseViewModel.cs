using MMUniGraduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.ViewModels
{
    public class IndexCourseViewModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Signature { get; set; }
        //public string Description { get; set; }

        //public string UserId { get; set; }
        public bool SkipCourse { get; set; }
        public bool ShowVideoMaterials { get; set; }
        public bool ShowTextMaterials { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        //public DateTime CourseStartDate { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        //public DateTime SkipCoursEndDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateTime { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<LectureFile> HWMaterials { get; set; }
    }
}
