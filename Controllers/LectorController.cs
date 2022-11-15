using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class LectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly ILectureService _lectureService;
        public LectorController(ApplicationDbContext context, ICourseService courseService, IStudyProgramService studyProgramService, ILectureService lectureService)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _lectureService = lectureService;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new AllCoursesViewModel
            {
                AllCourses = _context.Courses.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Assessment()
        {
            var viewModel = new AssessmentsViewModel
            {
                AllCourses = _context.Courses.ToList(),
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs(),
                Lectures = _lectureService.GetAllAsKeyValuePairs()
            };

            var lectures = new List<Lecture>(); 
            var homeworks = new List<Homework>();

            foreach (var course in viewModel.AllCourses)
            {
                lectures = _context.Lectures.Where(l => l.CourseId == course.Id).ToList();

                course.Lectures = lectures;

                foreach (var lecture in course.Lectures)
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();

                    lecture.Homeworks = homeworks;
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Assessment(AssessmentsViewModel input)
        {
            if (!ModelState.IsValid)
            {
                input.AllCourses = _context.Courses.ToList();
                input.StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs();
                input.Courses = _courseService.GetAllAsKeyValuePairs();
                input.Lectures = _lectureService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            var viewModel = new AssessmentsViewModel
            {
                AllCourses = _context.Courses.Where(c => c.Id == input.CourseId && c.StudyProgramId == input.ProgramId),
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs(),
                Lectures = _lectureService.GetAllAsKeyValuePairs()
            };

            var lectures = new List<Lecture>();
            var homeworks = new List<Homework>();

            foreach (var course in viewModel.AllCourses)
            {
                lectures = _context.Lectures.Where(l => l.CourseId == course.Id && l.Id == input.LectureId).ToList();
                course.Lectures = lectures;

                foreach (var lecture in course.Lectures)
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                    lecture.Homeworks = homeworks;
                }
            }
            return View(viewModel);
        }
    }
}
