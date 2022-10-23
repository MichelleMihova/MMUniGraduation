using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILectureService _lectureService;
        private readonly ICourseService _courseService;
        public LectureController(ApplicationDbContext context, ILectureService lectureService, ICourseService courseService)
        {
            _context = context;
            _lectureService = lectureService;
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
    }
}
