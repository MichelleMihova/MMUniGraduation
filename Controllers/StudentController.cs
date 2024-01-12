using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using System.Linq;
using MMUniGraduation.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using MMUniGraduation.ViewModels;
using AutoMapper;

namespace MMUniGraduation.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileExpression configuration;
        public StudentController(ApplicationDbContext context, ICourseService courseService, IStudentService studentService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studentService = studentService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var passedStudentCourses = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == true);
            var currentStudentCourses = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == false);

            var passedCoursesGrade = new Dictionary<Course, decimal>();
            var curr = new List<Course>();
            
            foreach (var item in passedStudentCourses)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == item.CourseId);
                passedCoursesGrade.Add(course, item.FinalGrade);
            }

            foreach (var item in currentStudentCourses)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == item.CourseId);
                curr.Add(course);
            }

            var photo = _context.Images.Where(x => x.StudentId == student.Id).Select(x => x.Id + '.' + x.Extension).FirstOrDefault();
            var viewModel = new IndexStudentViewModel
            {
                CurrentCourses = curr,
                Id = student.Id,
                UserId = user.Id,
                Image = photo,
                PassedCoursesGrade = passedCoursesGrade
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var viewModel = new EditStudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ShowTextMaterials = student.ShowTextMaterials,
                ShowVideoMaterials = student.ShowVideoMaterials
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditStudentViewModel input)
        {
            await _studentService.EditStudent(input);

            return RedirectToAction("Index","Student");
        }

    }
}
