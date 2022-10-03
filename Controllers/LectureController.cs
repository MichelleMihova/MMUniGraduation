using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Data;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System;
using System.Linq;
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
        [HttpPost]
        //public async Task<IActionResult> Create(string Name, string Description, string ParetntLectureSignature, string NextLectureSignature, DateTime DateTimeToShow, DateTime EndDateTimeForHW)
        public async Task<IActionResult> Create(CreateLecture input)
        {
            if (!ModelState.IsValid)
            {
                input.Courses = _courseService.GetAllAsKeyValuePairs();
                input.AllLectures = _lectureService.GetAllAsKeyValuePairs();
            }
            //var user = await _userManager.GetUserAsync(this.User);
            //await _studyProgramService.CreateAsync(input, user.Id);
            //await _lectureService.CreateAsync(Name, Description, ParetntLectureSignature, NextLectureSignature, DateTimeToShow, EndDateTimeForHW);
            await _lectureService.CreateLectureAsync(input);
            this.TempData["Message"] = "Lecture created successfully!";

            return RedirectToAction("Index", "Home");
            //return Content($" Name: {Name}, Desc: {Description}");
            //return View();
        }
        // GET: Course
        public async Task<IActionResult> Index1()
        {
            return View(await _context.Lectures.ToListAsync());
        }
    }
}
