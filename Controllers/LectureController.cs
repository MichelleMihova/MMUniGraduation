using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILectureService _lectureService;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public LectureController(ApplicationDbContext context, ILectureService lectureService, ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _lectureService = lectureService;
            _courseService = courseService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            //var courses = _context.Courses.Include(c => c.Name);
            //return View(courses.ToList());

            var viewModel = new CreateLecture
            {
                //Courses - from model/create/createLecture
                Courses = _courseService.GetAllAsKeyValuePairs(),
                AllLectures = _lectureService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLecture input)
        {
            if (!ModelState.IsValid)
            {
                input.Courses = _courseService.GetAllAsKeyValuePairs();
                input.AllLectures = _lectureService.GetAllAsKeyValuePairs();
            }
            //var user = await _userManager.GetUserAsync(this.User);
          
            await _lectureService.CreateLectureAsync(input);

            this.TempData["Message"] = "Lecture created successfully!";

            return RedirectToAction("Index", "Home");
        }
        // GET: Course
        public async Task<IActionResult> Index1()
        {
            return View(await _context.Lectures.ToListAsync());
        }

        public async Task<IActionResult> AddHomework(IFormFile file, int lectureId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var currLecture = await _context.Lectures.FirstOrDefaultAsync(x => x.Id == lectureId);

            await _lectureService.AddHomeworkToLecture(lectureId, file, user.Id);

             this.TempData["Message"] = "Homework added successfully!";

            
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "Course", new { courseId = 1 });
        }

        public async Task<IActionResult> EditHomework(int homeworkId, decimal homeworkGrade, string homeworkComment)
        {
            await _lectureService.EditHomework(homeworkId, homeworkGrade, homeworkComment);
            return RedirectToAction("Assessment", "Lector");
        }

        //public async Task<IActionResult> EditLecture(int lectureId, string lectureDescription, int courseId)
        public async Task<IActionResult> EditLecture(EditCourseViewModel input)
        {
            await _lectureService.EditLecture(input);
            return RedirectToAction("Edit", "Course", new { courseId = input.CourseId });
        }

        public async Task<IActionResult> DeleteLectureMaterial(string lectureFileId, int courseId)
        {
            await _lectureService.DeleteLectureMaterial(lectureFileId);
            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }
        
        public async Task<IActionResult> DeleteHomework(int lectureId, int courseId)
        {
            await _lectureService.DeleteHomework(lectureId);
            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }

        public async Task<IActionResult> DeleteLecture(int lectureId, int courseId)
        {
            await _lectureService.DeleteLectureMaterial(lectureId);
            await _lectureService.DeleteHomework(lectureId);
            await _lectureService.DeleteLecture(lectureId);

            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }

        public async Task<IActionResult> SetConstraints(string lectureFileId, int courseId)
        {

            return RedirectToAction("Edit", "Course", new { courseId = courseId });
        }
            
    }
}
