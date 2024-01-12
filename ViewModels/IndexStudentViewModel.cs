using AutoMapper;
using MMUniGraduation.Models;
using SmartProfil.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.ViewModels
{
    public class IndexStudentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Image { get; set; }
        public ICollection<Course> CurrentCourses { get; set; }
        public IDictionary<Course, decimal> PassedCoursesGrade { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Student, IndexStudentViewModel>()
        //        .ForMember(x => x.Image, opt =>
        //            opt.MapFrom(x =>
        //                "/img/" + x.Photos.FirstOrDefault().Id + "." + x.Photos.FirstOrDefault().Extension));
        //}


    }
}
