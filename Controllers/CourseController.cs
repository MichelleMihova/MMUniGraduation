using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudyProgramService _studyProgramService;
        private readonly IWebHostEnvironment _webHost;

        public CourseController(ICourseService courseService, IStudyProgramService studyProgramService, ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _webHost = webHost;
        }

        public FileResult DownloadFile(string fileName)
        {
            string path = Path.Combine(_webHost.WebRootPath, "files/") + fileName;

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
        public async Task<IActionResult> Index(int courseId)
        {
            var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            currentCourse.Lectures = await _context.Lectures.Where(l => l.CourseId == courseId).ToListAsync();

            var textMaterial = new List<LectureFile>();

            foreach (var lecture in currentCourse.Lectures)
            {
                textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id).ToList();

                foreach (var file in textMaterial)
                {
                    lecture.TextMaterials.Add(file);
                }
            }

            return View(currentCourse);

            //await _context.LectureFiles.Where(l => l.LectureId == courseId).ToListAsync()


            //foreach (var item in currentCourse.Lectures)
            //{

            //}


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

            //return View(await _context.Lectures.Where(l => l.CourseId == courseId).ToListAsync());
            //return View(viewModel);
            //return View();
            //await _context.StudyPrograms.ToListAsync()
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourse input)
        {
            if (!ModelState.IsValid)
            {
                input.StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            //var user = await _userManager.GetUserAsync(this.User);

            await _courseService.CreateCourseAsync(input);

            this.TempData["Message"] = "Course created successfully!";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AllCourses(int studyProgramId)
        {
            return View(await _context.Courses.Where(c => c.StudyProgramId == studyProgramId).ToListAsync());
        }

        public IActionResult Edit()
        {
            return this.View();
        }

    }
}
