using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
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
        private readonly ILectureService _lectureService;
        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseService courseService, IStudyProgramService studyProgramService,
            ILectureService lectureService, ApplicationDbContext context, IWebHostEnvironment webHost,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studyProgramService = studyProgramService;
            _lectureService = lectureService;
            _webHost = webHost;
            _userManager = userManager;
        }

        public FileResult DownloadFile(string fileName, string type)
        {
            string path;
            if (type == "homework")
            {
                path = Path.Combine(_webHost.WebRootPath, "homeworks/") + fileName;
            }
            else
            {
                path = Path.Combine(_webHost.WebRootPath, "files/") + fileName;
            }

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            currentCourse.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var textMaterial = new List<LectureFile>();

            foreach (var lecture in currentCourse.Lectures)
            {
                textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id).ToList();

                lecture.TextMaterials = textMaterial;
            }

            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var homework = new List<Homework>();
            foreach (var lecture in currentCourse.Lectures)
            {
                if (student != null)
                {
                    homework = _context.Homeworks.Where(l => l.LectureId == lecture.Id && l.StudentId == student.UserId).ToList();
                }
                
                lecture.Homeworks = homework;
            }

            //TO DO..
            this.TempData["Message"] = "You have been sucessfully assigned to this course !";

            return View(currentCourse);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs(),
                Courses = _courseService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin, Teacher")]
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

        [Authorize]
        public async Task<IActionResult> AllCourses(int studyProgramId, string message)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var viewModel = new AllCoursesViewModel
            {
                NextCourseName = _courseService.GetNextCourseSuggestion(student),
                AllCourses = await _context.Courses.Where(c => c.StudyProgramId == studyProgramId).ToListAsync()
            };

            if (message != null)
            {
                this.TempData["Message"] = message;
            }

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AssignUserToCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            if (student != null && student.CurrentCourseId == 0)
            {
                //student.CurrentCourse.Id = courseId;
                student.CurrentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
                _context.SaveChanges();
            }
            else if (student != null && student.CurrentCourseId != courseId)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == courseId);
                return RedirectToAction("AllCourses", new { studyProgramId = course.StudyProgramId, message = $"You are already assigned to {course.Name}" });
            }

            return RedirectToAction("Index", new { courseId = courseId });
        }

        public IActionResult Edit(int courseId)
        {
            //TO DO..
            //Add or Change criterias who and when can see the lectures

            //var currentCourse = _context.Courses.FirstOrDefault(x => x.Id == courseId);
            //currentCourse.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var editViewModel = new EditCourseViewModel
            {
                Course = _context.Courses.FirstOrDefault(x => x.Id == courseId),
                Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList()
            };

            //editViewModel.Course.Lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToList();

            var textMaterial = new List<LectureFile>();

            foreach (var lecture in editViewModel.Lectures)
            {
                textMaterial = _context.LectureFiles.Where(l => l.LectureId == lecture.Id).ToList();

                foreach (var file in textMaterial)
                {
                    lecture.TextMaterials.Add(file);
                }
            }

            editViewModel.Course.Lectures = editViewModel.Lectures;

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseViewModel input)
        {
            //TO DO..
            //Change description/grade/skip course end date/ criterias who and when can see lectures
            var course = _context.Courses.FirstOrDefault(x => x.Id == input.CourseId);
            if (input.CourseDescription != null)
            {
                course.Description = input.CourseDescription;
            }
            await _context.SaveChangesAsync();

            var editViewModel = new EditCourseViewModel
            {
                Course = _context.Courses.FirstOrDefault(x => x.Id == input.CourseId),
                Lectures = _context.Lectures.Where(l => l.CourseId == input.CourseId).ToList()
            };
            var currLecture = _context.Lectures.Where(l => l.CourseId == input.CourseId && l.Id == input.LectureId);

            await _lectureService.EditLecture(input);

            //return View(editViewModel);
            return RedirectToAction("Edit", new { courseId = input.CourseId });
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var lectures = _context.Lectures.Where(l => l.CourseId == courseId).ToArray();

            foreach (var lecture in lectures)
            {
                await _lectureService.DeleteLectureMaterial(lecture.Id);
                await _lectureService.DeleteHomework(lecture.Id);
                await _lectureService.DeleteLecture(lecture.Id);
            }

            await _courseService.DeleteCourse(courseId);

            return RedirectToAction("Index", "Lector");
        }
    }
}
