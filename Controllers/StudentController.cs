using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using System.Linq;
using MMUniGraduation.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MMUniGraduation.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudentController(ApplicationDbContext context, ICourseService courseService, IStudentService studentService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _courseService = courseService;
            _studentService = studentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //display image
            //var content = db.Contents.Select(s => new
            //{
            //    s.ID,
            //    s.Title,
            //    s.Image,
            //    s.Contents,
            //    s.Description
            //});
            //List<ContentViewModel> contentModel = content.Select(item => new ContentViewModel()
            //{
            //    ID = item.ID,
            //    Title = item.Title,
            //    Image = item.Image,
            //    Description = item.Description,
            //    Contents = item.Contents
            //}).ToList();
            //return View(contentModel);

            var user = await _userManager.GetUserAsync(this.User);
            var student = _context.Students.FirstOrDefault(x => x.UserId == user.Id);

            var passedStudentCourses = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == true);
            var currentStudentCourses = _context.StudentCourses.Where(x => x.StudentId == student.Id && x.IsPassed == false);

            var passedCoursesGrade = new Dictionary<Course, decimal>();
            //var pass = new List<Course>();
            var curr = new List<Course>();
            
            foreach (var item in passedStudentCourses)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == item.CourseId);
                //pass.Add(course);
                passedCoursesGrade.Add(course, item.FinalGrade);
            }

            foreach (var item in currentStudentCourses)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == item.CourseId);
                curr.Add(course);
            }

            var viewModel = new Student
            {
                //PassedCourses = pass,
                CurrentCourses = curr,
                Id = student.Id,
                UserId = user.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ShowTextMaterials = student.ShowTextMaterials,
                ShowVideoMaterials = student.ShowVideoMaterials,
                PassedCoursesGrade = passedCoursesGrade
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(Student input)
        {
            await _studentService.EditStudent(input);

            return RedirectToAction("Index","Student");
        }

    }
}
