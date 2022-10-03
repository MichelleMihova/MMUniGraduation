using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMUniGraduation.Models
{
    public class Criteria
    {
        public long MinGrade { get; set; }
        public int MinCountOfHW { get; set; }
        //List of students names who can see the lecture material
        public List<Student> StudentsNames { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ShowLectureMaterial { get; set; }
        //lecture IF or material ID
        public Lecture LectureID { get; set; }
    }
}
