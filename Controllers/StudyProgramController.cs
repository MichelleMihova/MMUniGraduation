using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class StudyProgramController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudyProgramService _studyProgramService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseService _courseService;
        private readonly ILectureService _lectureService;
        public StudyProgramController(ApplicationDbContext context, IStudyProgramService studyProgramService, UserManager<ApplicationUser> userManager,
            ICourseService courseService, ILectureService lectureService)
        {
            _context = context;
            _studyProgramService = studyProgramService;
            _userManager = userManager;
            _courseService = courseService;
            _lectureService = lectureService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                StudyPrograms = _studyProgramService.GetAllPrograms<IndexStudyProgramViewModel>()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStudyProgram model)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var programNames = _context.StudyPrograms.Select(x => x.Name);

            if (programNames.Contains(model.Name))
            {
                this.TempData["Message"] = "The program has not been created yet! There is an existing program with the same name!";
                return View(model);
            }

            if (model.Images == null)
            {
                this.TempData["Message"] = "The program has not been created yet! No image has been added!";
                return View(model);
            }

            await _studyProgramService.CreateAsync(model);

            this.TempData["Message"] = "Program created successfully!";

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int programId)
        {
            var courses = _context.Courses.Where(l => l.StudyProgramId == programId).ToArray();

            foreach (var course in courses)
            {
                var currentCourseCount = _context.StudentCourses.Where(x => x.CourseId == course.Id && x.IsPassed == false).Count();

                if (currentCourseCount != 0)
                {
                    this.TempData["Message"] = "Program could not be deleted when we have assigned users to it's courses!";

                    return RedirectToAction("EditCourses", "Lector");
                }
            }

            foreach (var course in courses)
            {
                await _courseService.DeleteSkippingCourseMaterial(course.Id);
                await _courseService.DeleteSkippingAssignment(course.Id);

                var lectures = _context.Lectures.Where(l => l.CourseId == course.Id).ToArray();

                foreach (var lecture in lectures)
                {
                    await _lectureService.DeleteLectureMaterial(lecture.Id);
                    await _lectureService.DeleteHomework(lecture.Id);
                    await _lectureService.DeleteLecture(lecture.Id);
                }

                await _courseService.DeleteCourse(course.Id);
            }

            await _studyProgramService.DeleteProgram(programId);

            return RedirectToAction("EditCourses", "Lector");
        }
    }
}
