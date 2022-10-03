using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var viewModel = new CreateCourse
            {
                StudyPrograms = _studyProgramService.GetAllAsKeyValuePairs()
            };

            return this.View(viewModel);
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
    }
}
