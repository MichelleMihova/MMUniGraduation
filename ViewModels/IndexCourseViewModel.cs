using MMUniGraduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.ViewModels
{
    public class IndexCourseViewModel
    {
        public bool SkipCourse { get; set; }
        public bool ShowVideoMaterials { get; set; }
        public bool ShowTextMaterials { get; set; }
        public string Image { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateTime { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
        public Lector Lector { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual Dictionary<int, List<LectureFile>> HomeworkMaterials { get; set;}
        public virtual StudentCourses StudentCourse { get; set; }
    }
}
