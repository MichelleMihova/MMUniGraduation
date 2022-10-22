using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudyProgramService _studyProgramService;

        public CourseController(ICourseService courseService, IStudyProgramService studyProgramService, ApplicationDbContext context)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            currentCourse.Lectures = await _context.Lectures.Where(l => l.CourseId == courseId).ToListAsync();

            //TO DO...
            // When upload file button is clicked -> to show in assesment page 

            //var viewModel1 = _context.Courses.Where(c => c.Id == courseId);

            //var viewModel = new Course
            //{
            //    //Courses = _courseService.GetAllAsKeyValuePairs()
            //    Lectures = await _context.Lectures.Where(l => l.CourseId == courseId).ToListAsync()
            //    //await _context.Courses.Where(c => c.Id == courseId).ToListAsync()
            //};
            //return View(await _context.Courses.Where(c => c.Id == courseId).ToListAsync());
            //await _context.Courses.Where(c => c.Id == courseId).ToListAsync()
            return View(currentCourse);
            //return View(await _context.Lectures.Where(l => l.CourseId == courseId).ToListAsync());
            //return View(viewModel);
            //return View();
            //await _context.StudyPrograms.ToListAsync()
        }
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        public IActionResult Edit()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourse input)
        {
            if (!ModelState.IsValid)
            {
                input.StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            await _courseService.CreateCourseAsync(input);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index", "Home");

            //var user = await _userManager.GetUserAsync(this.User);
            //await _studyProgramService.CreateAsync(input, user.Id);

        }
        //public async Task<IActionResult> AllCourses(int input)
        public async Task<IActionResult> AllCourses(int studyProgramId)
        {
            return View(await _context.Courses.Where(c => c.StudyProgramId == studyProgramId).ToListAsync());
            //return View(await _context.Courses.Where(c => c.StudyProgramId == 1).ToListAsync());
        }

    }
}
