using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public LectorController(ApplicationDbContext context, ICourseService courseService, 
            IStudyProgramService studyProgramService, ILectureService lectureService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _lectureService = lectureService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(this.User);

            var viewModel = new AllCoursesViewModel
            {
                AllCourses = _context.Courses.Where(x => x.CreatorId == user.Id).ToList()
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
            var students = new List<Student>();

            foreach (var course in viewModel.AllCourses)
            {
                lectures = _context.Lectures.Where(l => l.CourseId == course.Id).ToList();

                course.Lectures = lectures;
                
                foreach (var lecture in course.Lectures)
                {
                    homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                    lecture.Homeworks = homeworks;

                    var studentsId = _context.Homeworks.Select(x => x.StudentId).ToList();
                    foreach (var id in studentsId)
                    {
                        var student = _context.Students.FirstOrDefault(x => x.UserId == id);
                        if (!students.Contains(student))
                        {
                            students.Add(student);
                        }
                    }
                }
            }

            viewModel.Students = students;

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

            //TO DO..
            //Show assessments only for current user
            foreach (var course in viewModel.AllCourses)
            {
                lectures = _context.Lectures.Where(l => l.CourseId == course.Id && l.Id == input.LectureId).ToList();
                course.Lectures = lectures;

                foreach (var lecture in course.Lectures)
                {
                    if (input.KindOfAssessment.ToUpper() == "HOMEWORKS")
                    {
                        homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle != "EXAM").ToList();
                    }
                    else if (input.KindOfAssessment.ToUpper() == "EXAMSOLUTIONS")
                    {
                        homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0 && l.HomeworkTitle == "EXAM").ToList();
                    }
                    else
                    {
                        homeworks = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.Grade == 0).ToList();
                    }
                    
                    lecture.Homeworks = homeworks;
                }
            }
            return View(viewModel);
        }
    }
}
